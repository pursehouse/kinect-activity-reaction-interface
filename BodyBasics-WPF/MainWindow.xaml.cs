//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     The Kinect for Windows APIs used here are preliminary and subject to change
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace BodyBasicsWPF {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using System.Windows.Forms;
    using WindowsInput;
    using WindowsInput.Native;
    using System.Windows.Data;
    using System.Windows.Controls;
    using InputManager;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        /// <summary>
        /// Radius of drawn hand circles
        /// </summary>
        private const double HandSize = 30;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 3;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as closed
        /// </summary>
        private readonly Brush handClosedBrush = new SolidColorBrush(Color.FromArgb(128, 255, 0, 0));

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as opened
        /// </summary>
        private readonly Brush handOpenBrush = new SolidColorBrush(Color.FromArgb(128, 0, 255, 0));

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as in lasso (pointer) position
        /// </summary>
        private readonly Brush handLassoBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 255));

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        public readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        public readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently tracked
        /// </summary>
        public readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>        
        public readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        /// <summary>
        /// Drawing group for body rendering output
        /// </summary>
        private DrawingGroup bodyDrawingGroup;
        private DrawingGroup actionHistoryDrawingGroup;
        

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        private DrawingImage bodyImage;
        private DrawingImage actionsHistoryImage;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper = null;

        /// <summary>
        /// Reader for body frames
        /// </summary>
        private BodyFrameReader reader = null;

        /// <summary>
        /// Array for the bodies
        /// </summary>
        private Body[] bodies = null;

        /// <summary>
        /// Width of display (depth space)
        /// </summary>
        public int displayWidth;

        /// <summary>
        /// Height of display (depth space)
        /// </summary>
        public int displayHeight;

        /// <summary>
        /// The time of the first frame received
        /// </summary>
        private long startTime = 0;
        
        /// <summary>
        /// Current status text to display
        /// </summary>
        private string statusText = null;
        
        public string dogName = null;

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

        public UserConfig kari = null;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow() {

            this.kari = new UserConfig( this );

            // create a stopwatch for FPS calculation
            this.stopwatch = new Stopwatch();

            // for Alpha, one sensor is supported
            this.kinectSensor = KinectSensor.GetDefault();

            if( this.kinectSensor != null ) {
                // get the coordinate mapper
                this.coordinateMapper = this.kinectSensor.CoordinateMapper;

                // open the sensor
                this.kinectSensor.Open();

                // get the depth (display) extents
                FrameDescription frameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;
                this.displayWidth = frameDescription.Width;
                this.displayHeight = frameDescription.Width;

                this.bodies = new Body[this.kinectSensor.BodyFrameSource.BodyCount];

                // open the reader for the body frames
                this.reader = this.kinectSensor.BodyFrameSource.OpenReader();

                // set the status text
                this.StatusText = Properties.Resources.InitializingStatusTextFormat;
            } else {
                // on failure, set the status text
                this.StatusText = Properties.Resources.NoSensorStatusText;
            }

            // Create the drawing group we'll use for drawing
            this.bodyDrawingGroup = new DrawingGroup();

            this.actionHistoryDrawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.bodyImage = new DrawingImage(this.bodyDrawingGroup);

            this.actionsHistoryImage = new DrawingImage(this.actionHistoryDrawingGroup);

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
        public ImageSource BodyImage {
            get {
                return this.bodyImage;
            }
        }
        public ImageSource ActionsHistoryImage {
            get {
                return this.actionsHistoryImage;
            }
        }

        /// <summary>
        /// Gets or sets the current status text to display
        /// </summary>
        public string StatusText {
            get {
                return this.statusText;
            }

            set {
                if( this.statusText != value ) {
                    this.statusText = value;

                    // notify any bound elements that the text has changed
                    if( this.PropertyChanged != null ) {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("StatusText"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the current status text to display
        /// </summary>
        public string DogName {
            get {
                return this.dogName;
            }

            set {
                if( this.dogName != value ) {
                    this.dogName = value;

                    // notify any bound elements that the text has changed
                    if( this.PropertyChanged != null ) {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("DogName"));
                    }
                }
            }
        }

        

        /// <summary>
        /// Execute start up tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Loaded( object sender, RoutedEventArgs e ) {
            if( this.reader != null ) {
                this.reader.FrameArrived += this.Reader_FrameArrived;
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing( object sender, CancelEventArgs e ) {
            if( this.reader != null ) {
                // BodyFrameReder is IDisposable
                this.reader.Dispose();
                this.reader = null;
            }

            // Body is IDisposable
            if( this.bodies != null ) {
                foreach( Body body in this.bodies ) {
                    if( body != null ) {
                        //body.Dispose();
                    }
                }
            }

            if( this.kinectSensor != null ) {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }

        /// <summary>
        /// Handles the body frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_FrameArrived( object sender, BodyFrameArrivedEventArgs e ) {
            BodyFrameReference frameReference = e.FrameReference;
              
            if( this.startTime == 0 ) {
                this.startTime = (long)frameReference.RelativeTime.Seconds;
            }

            try {
                BodyFrame frame = frameReference.AcquireFrame();

                if( frame != null ) {
                    // BodyFrame is IDisposable
                    using( frame ) {
                        this.framesSinceUpdate++;

                        // update status unless last message is sticky for a while
                        if( DateTime.Now >= this.nextStatusUpdate ) {
                            // calcuate fps based on last frame received
                            double fps = 0.0;

                            if( this.stopwatch.IsRunning ) {
                                this.stopwatch.Stop();
                                fps = this.framesSinceUpdate / this.stopwatch.Elapsed.TotalSeconds;
                                this.stopwatch.Reset();
                            }

                            this.nextStatusUpdate = DateTime.Now + TimeSpan.FromSeconds(1);
                            this.StatusText = string.Format(Properties.Resources.StandardStatusTextFormat, fps, frameReference.RelativeTime.Seconds - this.startTime);
                            //if( this.DogName == null )
                            //this.DogName = (string)DateTime.Now.ToString(); 
                        }

                        if( !this.stopwatch.IsRunning ) {
                            this.framesSinceUpdate = 0;
                            this.stopwatch.Start();
                        }

                        using( 
                               DrawingContext bodyDraw          = this.bodyDrawingGroup.Open(),
                                              actionHistoryDraw = this.actionHistoryDrawingGroup.Open()
                            ) {
                            // Draw a transparent background to set the render size
                            bodyDraw.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, this.displayWidth, this.displayHeight));

                            // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                            // As long as those body objects are not disposed and not set to null in the array,
                            // those body objects will be re-used.
                            frame.GetAndRefreshBodyData(this.bodies);
                            
                            int bodyNum = 0;
                            foreach( Body body in this.bodies ) {
         
                                if( body.IsTracked ) {
                                    this.DrawClippedEdges(body, bodyDraw);

                                    IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                                    // convert the joint points to depth (display) space
                                    Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();
                                    foreach( JointType jointType in joints.Keys ) {
                                        DepthSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraPointToDepthSpace(joints[jointType].Position);
                                        jointPoints[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);
                                    }

                                    this.DrawBody(joints, jointPoints, bodyDraw, actionHistoryDraw );

                                    if ( ++bodyNum == 1) {
                                        this.kari.actionsDetect( frame, joints, jointPoints, bodyDraw, actionHistoryDraw );
                                    }
                                    
                                   // this.DrawHand(body.HandLeftState, jointPoints[JointType.HandLeft], bodyDraw);
                                   // this.DrawHand(body.HandRightState, jointPoints[JointType.HandRight], bodyDraw);

                                }
                            }

                            // prevent drawing outside of our render area
                            this.bodyDrawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, this.displayWidth, this.displayHeight));
                        }
                    }
                }
            }
            catch( Exception ) {
                // ignore if the frame is no longer available
            }
        }

        /// <summary>
        /// Draws a body
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawBody( IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, DrawingContext bodyDraw, DrawingContext actionHistoryDraw) {
            // Draw the bones

            // Torso
            this.DrawBone(joints, jointPoints, JointType.Head, JointType.Neck, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.Neck, JointType.SpineShoulder, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.SpineShoulder, JointType.SpineMid, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.SpineMid, JointType.SpineBase, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.SpineShoulder, JointType.ShoulderRight, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.SpineShoulder, JointType.ShoulderLeft, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.SpineBase, JointType.HipRight, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.SpineBase, JointType.HipLeft, bodyDraw);

            // Right Arm    
            this.DrawBone(joints, jointPoints, JointType.ShoulderRight, JointType.ElbowRight, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.ElbowRight, JointType.WristRight, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.WristRight, JointType.HandRight, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.HandRight, JointType.HandTipRight, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.WristRight, JointType.ThumbRight, bodyDraw);

            // Left Arm
            this.DrawBone(joints, jointPoints, JointType.ShoulderLeft, JointType.ElbowLeft, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.ElbowLeft, JointType.WristLeft, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.WristLeft, JointType.HandLeft, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.HandLeft, JointType.HandTipLeft, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.WristLeft, JointType.ThumbLeft, bodyDraw);

            // Right Leg
            this.DrawBone(joints, jointPoints, JointType.HipRight, JointType.KneeRight, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.KneeRight, JointType.AnkleRight, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.AnkleRight, JointType.FootRight, bodyDraw);

            // Left Leg
            this.DrawBone(joints, jointPoints, JointType.HipLeft, JointType.KneeLeft, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.KneeLeft, JointType.AnkleLeft, bodyDraw);
            this.DrawBone(joints, jointPoints, JointType.AnkleLeft, JointType.FootLeft, bodyDraw);

            // Draw the joints
            foreach( JointType jointType in joints.Keys ) {
                Brush drawBrush = null;

                TrackingState trackingState = joints[jointType].TrackingState;

                if( trackingState == TrackingState.Tracked ) {
                    drawBrush = this.trackedJointBrush;
                } else if( trackingState == TrackingState.Inferred ) {
                    drawBrush = this.inferredJointBrush;
                }

                if( drawBrush != null ) {
                    bodyDraw.DrawEllipse(drawBrush, null, jointPoints[jointType], JointThickness, JointThickness);
                }
            }
        }

        /// <summary>
        /// Draws one bone of a body (joint to joint)
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="jointType0">first joint of bone to draw</param>
        /// <param name="jointType1">second joint of bone to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawBone( IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, JointType jointType0, JointType jointType1, DrawingContext bodyDraw ) {
            Joint joint0 = joints[jointType0];
            Joint joint1 = joints[jointType1];

            // If we can't find either of these joints, exit
            if( joint0.TrackingState == TrackingState.NotTracked ||
                joint1.TrackingState == TrackingState.NotTracked ) {
                return;
            }

            // Don't draw if both points are inferred
            if( joint0.TrackingState == TrackingState.Inferred &&
                joint1.TrackingState == TrackingState.Inferred ) {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = this.inferredBonePen;
            if( ( joint0.TrackingState == TrackingState.Tracked ) && ( joint1.TrackingState == TrackingState.Tracked ) ) {
                drawPen = this.trackedBonePen;
            }

            bodyDraw.DrawLine(drawPen, jointPoints[jointType0], jointPoints[jointType1]);
        }

        /// <summary>
        /// Draws a hand symbol if the hand is tracked: red circle = closed, green circle = opened; blue circle = lasso
        /// </summary>
        /// <param name="handState">state of the hand</param>
        /// <param name="handPosition">position of the hand</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawHand( HandState handState, Point handPosition, DrawingContext bodyDraw ) {
            switch( handState ) {
                case HandState.Closed:
                    bodyDraw.DrawEllipse(this.handClosedBrush, null, handPosition, HandSize, HandSize);
                    break;

                case HandState.Open:
                    bodyDraw.DrawEllipse(this.handOpenBrush, null, handPosition, HandSize, HandSize);
                    break;

                case HandState.Lasso:
                    bodyDraw.DrawEllipse(this.handLassoBrush, null, handPosition, HandSize, HandSize);
                    break;
            }
        }

        /// <summary>
        /// Draws indicators to show which edges are clipping body data
        /// </summary>
        /// <param name="body">body to draw clipping information for</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawClippedEdges( Body body, DrawingContext bodyDraw ) {
            FrameEdges clippedEdges = body.ClippedEdges;

            if( clippedEdges.HasFlag(FrameEdges.Bottom) ) {
                bodyDraw.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, this.displayHeight - ClipBoundsThickness, this.displayWidth, ClipBoundsThickness));
            }

            if( clippedEdges.HasFlag(FrameEdges.Top) ) {
                bodyDraw.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, this.displayWidth, ClipBoundsThickness));
            }

            if( clippedEdges.HasFlag(FrameEdges.Left) ) {
                bodyDraw.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, this.displayHeight));
            }

            if( clippedEdges.HasFlag(FrameEdges.Right) ) {
                bodyDraw.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(this.displayWidth - ClipBoundsThickness, 0, ClipBoundsThickness, this.displayHeight));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.kari = new UserConfig(this);
        }

    }
}
