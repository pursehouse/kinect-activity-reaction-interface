//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     The Kinect for Windows APIs used here are preliminary and subject to change
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace CoordinateMappingBasicsWPF
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
        /// Indicates opaque in an opacity mask
        /// </summary>
        private const int OpaquePixel = -1;

        /// <summary>
        /// Size of the RGB pixel in the bitmap
        /// </summary>
        private readonly int bytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper = null;

        /// <summary>
        /// Reader for depth/color/body index frames
        /// </summary>
        private MultiSourceFrameReader reader = null;

        /// <summary>
        /// Bitmap to display
        /// </summary>
        private WriteableBitmap bitmap = null;

        /// <summary>
        /// Intermediate storage for receiving depth frame data from the sensor
        /// </summary>
        private ushort[] depthFrameData = null;

        /// <summary>
        /// Intermediate storage for receiving color frame data from the sensor
        /// </summary>
        private byte[] colorFrameData = null;

        /// <summary>
        /// Intermediate storage for receiving body index frame data from the sensor
        /// </summary>
        private byte[] bodyIndexFrameData = null;

        /// <summary>
        /// Intermediate storage for frame data converted to color
        /// </summary>
        private byte[] displayPixels = null;

        /// <summary>
        /// Intermediate storage for the depth to color mapping
        /// </summary>
        private ColorSpacePoint[] colorPoints = null;

        /// <summary>
        /// The time of the first frame received
        /// </summary>
        private long startTime = 0;

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
            this.kinectSensor = KinectSensor.Default;

            if (this.kinectSensor != null)
            {
                // get the coordinate mapper
                this.coordinateMapper = this.kinectSensor.CoordinateMapper;

                // open the sensor
                this.kinectSensor.Open();

                FrameDescription depthFrameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;

                int depthWidth = depthFrameDescription.Width;
                int depthHeight = depthFrameDescription.Height;

                // allocate space to put the pixels being received and converted
                this.depthFrameData = new ushort[depthWidth * depthHeight];
                this.bodyIndexFrameData = new byte[depthWidth * depthHeight];
                this.displayPixels = new byte[depthWidth * depthHeight * this.bytesPerPixel];
                this.colorPoints = new ColorSpacePoint[depthWidth * depthHeight];

                // create the bitmap to display
                this.bitmap = new WriteableBitmap(depthWidth, depthHeight, 96.0, 96.0, PixelFormats.Bgra32, null);

                FrameDescription colorFrameDescription = this.kinectSensor.ColorFrameSource.FrameDescription;

                int colorWidth = colorFrameDescription.Width;
                int colorHeight = colorFrameDescription.Height;

                // allocate space to put the pixels being received
                this.colorFrameData = new byte[colorWidth * colorHeight * this.bytesPerPixel];

                this.reader = this.kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Depth | FrameSourceTypes.Color | FrameSourceTypes.BodyIndex);

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
                this.reader.MultiSourceFrameArrived += this.Reader_MultiSourceFrameArrived;
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
                // MultiSourceFrameReder is IDisposable
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
            // create a render target that we'll render our composite control to
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)CompositeImage.ActualWidth, (int)CompositeImage.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush brush = new VisualBrush(CompositeImage);
                dc.DrawRectangle(brush, null, new Rect(new Point(), new Size(CompositeImage.ActualWidth, CompositeImage.ActualHeight)));
            }

            renderBitmap.Render(dv);

            // create a png bitmap encoder which knows how to save a .png file
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            string time = System.DateTime.Now.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);

            string myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            string path = Path.Combine(myPhotos, "KinectScreenshot-CoordinateMapping-" + time + ".png");

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

        /// <summary>
        /// Handles the depth/color/body index frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            MultiSourceFrameReference frameReference = e.FrameReference;

            MultiSourceFrame multiSourceFrame = frameReference.AcquireFrame();
            DepthFrame depthFrame = null;
            ColorFrame colorFrame = null;
            BodyIndexFrame bodyIndexFrame = null;

            try
            {
                multiSourceFrame = frameReference.AcquireFrame();

                if (multiSourceFrame != null)
                {
                    // MultiSourceFrame is IDisposable
                    using (multiSourceFrame)
                    {
                        DepthFrameReference depthFrameReference = multiSourceFrame.DepthFrameReference;
                        ColorFrameReference colorFrameReference = multiSourceFrame.ColorFrameReference;
                        BodyIndexFrameReference bodyIndexFrameReference = multiSourceFrame.BodyIndexFrameReference;

                        if (this.startTime == 0)
                        {
                            this.startTime = depthFrameReference.RelativeTime;
                        }

                        depthFrame = depthFrameReference.AcquireFrame();
                        colorFrame = colorFrameReference.AcquireFrame();
                        bodyIndexFrame = bodyIndexFrameReference.AcquireFrame();

                        if ((depthFrame != null) && (colorFrame != null) && (bodyIndexFrame != null))
                        {
                            this.framesSinceUpdate++;

                            FrameDescription depthFrameDescription = depthFrame.FrameDescription;
                            FrameDescription colorFrameDescription = colorFrame.FrameDescription;
                            FrameDescription bodyIndexFrameDescription = bodyIndexFrame.FrameDescription;

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
                                this.StatusText = string.Format(Properties.Resources.StandardStatusTextFormat, fps, depthFrameReference.RelativeTime - this.startTime);
                            }

                            if (!this.stopwatch.IsRunning)
                            {
                                this.framesSinceUpdate = 0;
                                this.stopwatch.Start();
                            }

                            int depthWidth = depthFrameDescription.Width;
                            int depthHeight = depthFrameDescription.Height;

                            int colorWidth = colorFrameDescription.Width;
                            int colorHeight = colorFrameDescription.Height;

                            int bodyIndexWidth = bodyIndexFrameDescription.Width;
                            int bodyIndexHeight = bodyIndexFrameDescription.Height;

                            // verify data and write the new registered frame data to the display bitmap
                            if (((depthWidth * depthHeight) == this.depthFrameData.Length) &&
                                ((colorWidth * colorHeight * this.bytesPerPixel) == this.colorFrameData.Length) &&
                                ((bodyIndexWidth * bodyIndexHeight) == this.bodyIndexFrameData.Length))
                            {
                                depthFrame.CopyFrameDataToArray(this.depthFrameData);
                                if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
                                {
                                    colorFrame.CopyRawFrameDataToArray(this.colorFrameData);
                                }
                                else
                                {
                                    colorFrame.CopyConvertedFrameDataToArray(this.colorFrameData, ColorImageFormat.Bgra);
                                }

                                bodyIndexFrame.CopyFrameDataToArray(this.bodyIndexFrameData);

                                this.coordinateMapper.MapDepthFrameToColorSpace(this.depthFrameData, this.colorPoints);

                                Array.Clear(this.displayPixels, 0, this.displayPixels.Length);

                                // loop over each row and column of the depth
                                for (int y = 0; y < depthHeight; ++y)
                                {
                                    for (int x = 0; x < depthWidth; ++x)
                                    {
                                        // calculate index into depth array
                                        int depthIndex = (y * depthWidth) + x;

                                        byte player = this.bodyIndexFrameData[depthIndex];

                                        // if we're tracking a player for the current pixel, sets its color and alpha to full
                                        if (player != 0xff)
                                        {
                                            // retrieve the depth to color mapping for the current depth pixel
                                            ColorSpacePoint colorPoint = this.colorPoints[depthIndex];

                                            // make sure the depth pixel maps to a valid point in color space
                                            int colorX = (int)Math.Floor(colorPoint.X + 0.5);
                                            int colorY = (int)Math.Floor(colorPoint.Y + 0.5);
                                            if ((colorX >= 0) && (colorX < colorWidth) && (colorY >= 0) && (colorY < colorHeight))
                                            {
                                                // calculate index into color array
                                                int colorIndex = ((colorY * colorWidth) + colorX) * this.bytesPerPixel;

                                                // set source for copy to the color pixel
                                                int displayIndex = depthIndex * this.bytesPerPixel;
                                                this.displayPixels[displayIndex] = this.colorFrameData[colorIndex];
                                                this.displayPixels[displayIndex + 1] = this.colorFrameData[colorIndex + 1];
                                                this.displayPixels[displayIndex + 2] = this.colorFrameData[colorIndex + 2];
                                                this.displayPixels[displayIndex + 3] = 0xff;
                                            }
                                        }
                                    }
                                }

                                this.bitmap.WritePixels(
                                    new Int32Rect(0, 0, depthWidth, depthHeight),
                                    this.displayPixels,
                                    depthWidth * this.bytesPerPixel,
                                    0);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignore if the frame is no longer available
            }
            finally
            {
                // MultiSourceFrame, DepthFrame, ColorFrame, BodyIndexFrame are IDispoable
                if (depthFrame != null)
                {
                    depthFrame.Dispose();
                    depthFrame = null;
                }

                if (colorFrame != null)
                {
                    colorFrame.Dispose();
                    colorFrame = null;
                }

                if (bodyIndexFrame != null)
                {
                    bodyIndexFrame.Dispose();
                    bodyIndexFrame = null;
                }

                if (multiSourceFrame != null)
                {
                    multiSourceFrame.Dispose();
                    multiSourceFrame = null;
                }
            }
        }
    }
}
