//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     The Kinect for Windows APIs used here are preliminary and subject to change
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace InfraredBasicsWPF
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Size of the RGB pixel in the bitmap
        /// </summary>
        private readonly int bytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Reader for infrared frames
        /// </summary>
        private InfraredFrameReader reader = null;

        /// <summary>
        /// Bitmap to display
        /// </summary>
        private WriteableBitmap bitmap = null;

        /// <summary>
        /// Intermediate storage for receiving frame data from the sensor
        /// </summary>
        private ushort[] frameData = null;

        /// <summary>
        /// Intermediate storage for frame data converted to color
        /// </summary>
        private byte[] pixels = null;

        /// <summary>
        /// The time of the first frame received
        /// </summary>
        private TimeSpan startTime;

        /// <summary>
        /// Current status text to display
        /// </summary>
        private string statusText = null;

        /// <summary>
        /// Next time to update FPS/frame time status
        /// </summary>
        private DateTime nextStatusUpdate = DateTime.MinValue;

        /// <summary>
        /// Number of frames since last FPS/frame time status
        /// </summary>
        private uint framesSinceUpdate = 0;

        /// <summary>
        /// Timer for FPS calculation
        /// </summary>
        private Stopwatch stopwatch = null;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            // create a stopwatch for FPS calculation
            this.stopwatch = new Stopwatch();

            // for Alpha, one sensor is supported
            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {
                // open the sensor
                this.kinectSensor.Open();

                FrameDescription frameDescription = this.kinectSensor.InfraredFrameSource.FrameDescription;

                // open the reader for the infrared frames
                this.reader = this.kinectSensor.InfraredFrameSource.OpenReader();

                // allocate space to put the pixels being received and converted
                this.frameData = new ushort[frameDescription.Width * frameDescription.Height];
                this.pixels = new byte[frameDescription.Width * frameDescription.Height * this.bytesPerPixel];

                // create the bitmap to display
                this.bitmap = new WriteableBitmap(frameDescription.Width, frameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);

                // set the status text
                this.StatusText = Properties.Resources.InitializingStatusTextFormat;
            }
            else
            {
                // on failure, set the status text
                this.StatusText = Properties.Resources.NoSensorStatusText;
            }

            // use the window object as the view model in this simple example
            this.DataContext = this;

            // initialize the components (controls) of the window
            this.InitializeComponent();
        }

        /// <summary>
        /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the bitmap to display
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return this.bitmap;
            }
        }

        /// <summary>
        /// Gets or sets the current status text to display
        /// </summary>
        public string StatusText
        {
            get
            {
                return this.statusText;
            }

            set
            {
                if (this.statusText != value)
                {
                    this.statusText = value;

                    // notify any bound elements that the text has changed
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("StatusText"));
                    }
                }
            }
        }

        /// <summary>
        /// Execute start up tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.reader != null)
            {
                this.reader.FrameArrived += this.Reader_FrameArrived;
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.reader != null)
            {
                // InfraredFrameReder is IDisposable
                this.reader.Dispose();
                this.reader = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }

        /// <summary>
        /// Handles the user clicking on the screenshot button
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void ScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.bitmap != null)
            {
                // create a png bitmap encoder which knows how to save a .png file
                BitmapEncoder encoder = new PngBitmapEncoder();

                // create frame from the writable bitmap and add to encoder
                encoder.Frames.Add(BitmapFrame.Create(this.bitmap));

                string time = System.DateTime.Now.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);

                string myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                string path = Path.Combine(myPhotos, "KinectScreenshot-Infrared-" + time + ".png");

                // write the new file to disk
                try
                {
                    // FileStream is IDisposable
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        encoder.Save(fs);
                    }

                    this.StatusText = string.Format(Properties.Resources.SavedScreenshotStatusTextFormat, path);
                    this.nextStatusUpdate = DateTime.Now + TimeSpan.FromSeconds(5);
                }
                catch (IOException)
                {
                    this.StatusText = string.Format(Properties.Resources.FailedScreenshotStatusTextFormat, path);
                    this.nextStatusUpdate = DateTime.Now + TimeSpan.FromSeconds(5);
                }
            }
        }

        /// <summary>
        /// Handles the infrared frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_FrameArrived(object sender, InfraredFrameArrivedEventArgs e)
        {
            InfraredFrameReference frameReference = e.FrameReference;

            if (this.startTime == null)
            {
                this.startTime = frameReference.RelativeTime;
            }

            try
            {
                InfraredFrame frame = frameReference.AcquireFrame();

                if (frame != null)
                {
                    // InfraredFrame is IDisposable
                    using (frame)
                    {
                        this.framesSinceUpdate++;

                        FrameDescription frameDescription = frame.FrameDescription;

                        // update status unless last message is sticky for a while
                        if (DateTime.Now >= this.nextStatusUpdate)
                        {
                            // calcuate fps based on last frame received
                            double fps = 0.0;

                            if (this.stopwatch.IsRunning)
                            {
                                this.stopwatch.Stop();
                                fps = this.framesSinceUpdate / this.stopwatch.Elapsed.TotalSeconds;
                                this.stopwatch.Reset();
                            }

                            this.nextStatusUpdate = DateTime.Now + TimeSpan.FromSeconds(1);
                            this.StatusText = string.Format(Properties.Resources.StandardStatusTextFormat, fps, frameReference.RelativeTime - this.startTime);
                        }

                        if (!this.stopwatch.IsRunning)
                        {
                            this.framesSinceUpdate = 0;
                            this.stopwatch.Start();
                        }

                        // verify data and write the new infrared frame data to the display bitmap
                        if (((frameDescription.Width * frameDescription.Height) == this.frameData.Length) &&
                            (frameDescription.Width == this.bitmap.PixelWidth) && (frameDescription.Height == this.bitmap.PixelHeight))
                        {
                            // Copy the pixel data from the image to a temporary array
                            frame.CopyFrameDataToArray(this.frameData);

                            // Convert the infrared to RGB
                            int colorPixelIndex = 0;
                            for (int i = 0; i < this.frameData.Length; ++i)
                            {
                                // Get the infrared value for this pixel
                                ushort ir = this.frameData[i];

                                // To convert to a byte, we're discarding the most-significant
                                // rather than least-significant bits.
                                // We're preserving detail, although the intensity will "wrap."

                                // To convert to a byte, we're discarding the least-significant bits.
                                byte intensity = (byte)(ir >> 8);

                                // Write out blue byte
                                this.pixels[colorPixelIndex++] = intensity;

                                // Write out green byte
                                this.pixels[colorPixelIndex++] = intensity;

                                // Write out red byte                        
                                this.pixels[colorPixelIndex++] = intensity;

                                // We're outputting BGR, the last byte in the 32 bits is unused so skip it
                                // If we were outputting BGRA, we would write alpha here.
                                ++colorPixelIndex;
                            }

                            this.bitmap.WritePixels(
                                new Int32Rect(0, 0, frameDescription.Width, frameDescription.Height),
                                this.pixels,
                                frameDescription.Width * this.bytesPerPixel,
                                0);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignore if the frame is no longer available
            }
        }
    }
}
