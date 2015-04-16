using Newtonsoft.Json.Linq;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using InputManager;

namespace BodyBasicsWPF {

    using Microsoft.Kinect;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Media;
    using WindowsInput;
    using WindowsInput.Native;

    public class UserConfig {

        public class readyActionItem {
            public bodyActionTypes bodyAction;
            public actionDetectTypes detectType;
            public float distanceMin;
            public float distanceMax;
            public int angleMin;
            public int angleMax;
            public actionExecuteActions executeAction;
            //public VirtualKeyCode vKeyCode = VirtualKeyCode.VK_0;
            public System.Windows.Forms.Keys vKeyCode;
            public actionDetectDirections direction;
            public actionDetectPlanes plane;
            public JointType jointBase;
            public JointType jointEnd;
        }

        private void MoveCursor( int x, int y ) {
            Cursor.Position = new System.Drawing.Point( x, y );
        }

        public enum bodyActions {
            BA_LEAN_LEFT = 1, //    lean_left                    inches body lean left
            BA_LEAN_RIGHT, //    lean_right                    inches body lean right
            BA_LEAN_FORWARD, //    lean_forwards                inches body lean forward
            BA_LEAN_BACKWARD, //    lean_backwards                inches body lean back
            BA_TURN_LEFT, //    turn_left                    angular amount of left body turn (degrees)
            BA_TURN_RIGHT, //    turn_right                    angular amount of right body turn(degrees)
            BA_HAND_LEFT_EXPAND, //                                left hand action. like expand or clench
            BA_HAND_LEFT_FORWARD, //    left_arm_forwards            forward distance from left hand to shoulder (inches)
            BA_HAND_LEFT_DOWN, //    left_arm_down                downward distance from left hand to shoulder (inches)
            BA_HAND_LEFT_UP, //    left_arm_up                    upward distance from left hand to shoulder (inches)
            BA_HAND_LEFT_OUT, //    left_arm_out                sideways distance from left hand to shoulder (inches)
            BA_HAND_LEFT_ACROSS, //    left_arm_across                sideways distance from left hand across body to shoulder (inches)
            BA_HAND_LEFT_BACKWARD,
            BA_HAND_RIGHT_EXPAND, //                                right hand action. like expand or clench
            BA_HAND_RIGHT_FORWARD, //    right_arm_forwards            forward distance from right hand to shoulder (inches)
            BA_HAND_RIGHT_DOWN, //    right_arm_down                downward distance from right hand to shoulder (inches)
            BA_HAND_RIGHT_UP, //    right_arm_up                upward distance from right hand to shoulder (inches)
            BA_HAND_RIGHT_OUT, //    right_arm_out                sideways distance from right hand to shoulder (inches)
            BA_HAND_RIGHT_ACROSS, //    right_arm_across            sideways distance from right hand across body to shoulder (inches)
            BA_HAND_RIGHT_BACKWARD,
            BA_FOOT_LEFT_FORWARD, //    left_foot_forwards            forward distance from left hip to foot (inches)
            BA_FOOT_LEFT_OUT, //    left_foot_sideways            sideways distance from left hip to foot (inches)
            BA_FOOT_LEFT_BACKWARD, //    left_foot_backwards            backwards distance from left hip to foot (inches)
            BA_FOOT_LEFT_UP, //    left_foot_up                height of left foot above other foot on ground (inches)
            BA_FOOT_LEFT_ACROSS, //    right foot acro
            BA_FOOT_RIGHT_FORWARD, //    right_foot_forwards            forward distance from right hip to foot (inches)
            BA_FOOT_RIGHT_OUT, //    right_foot_sideways            sideways distance from right hip to foot (inches)
            BA_FOOT_RIGHT_BACKWARD, //    right_foot_backwards        backwards distance from right hip to foot (inches)
            BA_FOOT_RIGHT_UP, //    right_foot_up                height of right foot above other foot on ground (inches)
            BA_FOOT_RIGHT_ACROSS, //    right foot acro
            BA_JUMP, //    jump                        height of both feet above ground (inches)
            BA_CROUCH, //    crouch                        crouch distance = 1, calculated as current height subtracted from standing height (inches)
            BA_WALK //    walk                        height of each step above ground when walking in place (inches)
        }

        public enum bodyParts {
            BP_NECK = 1,
            BP_TORSO_UPPER,
            BP_TORSO_LOWER,
            BP_ARM_UPPER_LEFT,
            BP_ARM_UPPER_RIGHT,
            BP_ARM_FOR_LEFT,
            BP_ARM_FOR_RIGHT,
            BP_SHOULDER_LEFT,
            BP_SHOULDER_RIGHT,
            BP_HIP_LEFT,
            BP_HIP_RIGHT,
            BP_TIBIA_LEFT,
            BP_TIBIA_RIGHT,
            BP_TARSUS_LEFT,
            BP_TARSUS_RIGHT
        }

        public enum actionSetParts {
            AS_REQUIRE = 1,
            AS_EXECUTE,
            AS_BODY_ACTION,

            AS_USE_DISTANCE,
            AS_DISTANCE_MIN,
            AS_DISTANCE_MAX,

            AS_USE_ANGLE,
            AS_ANGLE_MIN,
            AS_ANGLE_MAX,
            AS_ANGLE_MIN_X,
            AS_ANGLE_MAX_X,
            AS_ANGLE_MIN_Y,
            AS_ANGLE_MAX_Y,

            AS_KEY_TAP,
            AS_KEY_HOLD,
            AS_KEY_PRESS,
            AS_KEY_RELEASE,

            AS_MOUSE_TRACK,
            AS_MOUSE_TAP,
            AS_MOUSE_HOLD,

            AS_WINDOW_HOLD,
            AS_CONFIG_LOAD,

            AS_WINDOW_SHOW,

            AS_JOINT_PREV_X,
            AS_JOINT_PREV_Y,
            AS_JOINT_PREV_ANGLE_X,
            AS_JOINT_PREV_ANGLE_Y
        }

        public enum actionCommandValues {
            ACV_MOUSE_LEFT = 1,
            ACV_MOUSE_RIGHT,

            ACV_ABSOLUTE,
            ACV_RELATIVE,
            ACV_DRAG,
            ACV_PUSH,

            ACV_MAXIMIZE,
            ACV_MINIMIZE,
            ACV_RESTORE,

            ACV_HAND_CLENCH,
            ACV_HAND_EXPAND
        }

        public enum actionPartStrings {
            require = actionSetParts.AS_REQUIRE,
            execute = actionSetParts.AS_EXECUTE,
            bodyAction = actionSetParts.AS_BODY_ACTION,
            useDistance = actionSetParts.AS_USE_DISTANCE,
            distanceMin = actionSetParts.AS_DISTANCE_MIN,
            distanceMax = actionSetParts.AS_DISTANCE_MAX,
            useAngle = actionSetParts.AS_USE_ANGLE,
            angleMin = actionSetParts.AS_ANGLE_MIN,
            angleMax = actionSetParts.AS_ANGLE_MAX,
            angleMinX = actionSetParts.AS_ANGLE_MIN_X,
            angleMaxX = actionSetParts.AS_ANGLE_MAX_X,
            angleMinY = actionSetParts.AS_ANGLE_MIN_Y,
            angleMaxY = actionSetParts.AS_ANGLE_MAX_Y,
            keyTap = actionSetParts.AS_KEY_TAP,
            keyHold = actionSetParts.AS_KEY_HOLD,
            keyPress = actionSetParts.AS_KEY_PRESS,
            keyRelease = actionSetParts.AS_KEY_RELEASE,
            mouseTrack = actionSetParts.AS_MOUSE_TRACK,
            mouseTap = actionSetParts.AS_MOUSE_TAP,
            mouseHold = actionSetParts.AS_MOUSE_HOLD,
            windowHold = actionSetParts.AS_WINDOW_HOLD,
            configLoad = actionSetParts.AS_CONFIG_LOAD,
            windowShow = actionSetParts.AS_WINDOW_SHOW,
        }

        public enum bodyActionStrings {
            leanLeft = bodyActions.BA_LEAN_LEFT,
            leanRight = bodyActions.BA_LEAN_RIGHT,
            leanForward = bodyActions.BA_LEAN_FORWARD,
            leanBackward = bodyActions.BA_LEAN_BACKWARD,

            turnLeft = bodyActions.BA_TURN_LEFT,
            turnRight = bodyActions.BA_TURN_RIGHT,

            handLeftExpand = bodyActions.BA_HAND_LEFT_EXPAND,
            handLeftForward = bodyActions.BA_HAND_LEFT_FORWARD,
            handLeftDown = bodyActions.BA_HAND_LEFT_DOWN,
            handLeftUp = bodyActions.BA_HAND_LEFT_UP,
            handLeftOut = bodyActions.BA_HAND_LEFT_OUT,
            handLeftAcross = bodyActions.BA_HAND_LEFT_ACROSS,
            handLeftBackward = bodyActions.BA_HAND_LEFT_BACKWARD,

            handRightExpand = bodyActions.BA_HAND_RIGHT_EXPAND,
            handRightForward = bodyActions.BA_HAND_RIGHT_FORWARD,
            handRightDown = bodyActions.BA_HAND_RIGHT_DOWN,
            handRightUp = bodyActions.BA_HAND_RIGHT_UP,
            handRightOut = bodyActions.BA_HAND_RIGHT_OUT,
            handRightAcross = bodyActions.BA_HAND_RIGHT_ACROSS,
            handRightBackward = bodyActions.BA_HAND_RIGHT_BACKWARD,

            footLeftForward = bodyActions.BA_FOOT_LEFT_FORWARD,
            footLeftOut = bodyActions.BA_FOOT_LEFT_OUT,
            footLeftBackward = bodyActions.BA_FOOT_LEFT_BACKWARD,
            footLeftUp = bodyActions.BA_FOOT_LEFT_UP,
            footLeftAcross = bodyActions.BA_FOOT_LEFT_ACROSS,

            footRightForward = bodyActions.BA_FOOT_RIGHT_FORWARD,
            footRightOut = bodyActions.BA_FOOT_RIGHT_OUT,
            footRightBackward = bodyActions.BA_FOOT_RIGHT_BACKWARD,
            footRightUp = bodyActions.BA_FOOT_RIGHT_UP,
            footRightAcross = bodyActions.BA_FOOT_RIGHT_ACROSS,

            jump = bodyActions.BA_JUMP,
            crouch = bodyActions.BA_CROUCH,
            walk = bodyActions.BA_WALK,
        }

        public string[] configFiles = new string[] { };

        internal SortedList<int, float> bodyPartLengths = new SortedList<int, float>();
        internal SortedList<int, SortedList<int, float>> bodyPartLengthFrames = new SortedList<int, SortedList<int, float>>();

        //public SortedList<int, SortedList<int, SortedList<int, SortedList<int, int>>>> actionSets = new SortedList<int, SortedList<int, SortedList<int, SortedList<int, int>>>>();

        public int[][][][] actionSets = new int[][][][] { };
        
        public SortedList<int, bool> actionSetsStatus = new SortedList<int, bool>();
        public List<SortedList<int, bool>> actionSetsStatusHistory = new List<SortedList<int, bool>>();
        

        public BodyFrame previousFrame;
        public IReadOnlyDictionary<JointType, Joint> previousJoints;
        public Dictionary<JointType, Point> previousJointPoints;

        public int personHeight = 0;
        public int actionsCount = 0;

        public int cameraAngleTest = 0;
        public int cameraAnglePrev = 0;
        public float cameraAngleAverage = 0F;
        public int cameraAngleTotal = 0;
        public int cameraAngleCount = 0;

        internal partial class DefineConstants {
            public const double PI = 3.1415926535897932384;
        }

        public void main() {
        }

        public object[] myActions = {
        };

        private void WalkNode( JToken node, Action<JObject> objectAction = null, Action<JProperty> propertyAction = null ) {
            if (node.Type == JTokenType.Object) {
                if (objectAction != null)
                    objectAction( (JObject)node );
                foreach (JProperty child in node.Children<JProperty>()) {
                    if (propertyAction != null)
                        propertyAction( child );
                    WalkNode( child.Value, objectAction, propertyAction );
                }
            } else if (node.Type == JTokenType.Array) {
                foreach (JToken child in node.Children()) {
                    WalkNode( child, objectAction, propertyAction );
                }
            }
        }

        //public readyActionItem[] readyActionItems;
        public List<DetectData> readyActionItems = new List<DetectData>();

        public void loadConfig( string configFileName ) {
            //this.myActions.Clear();

            string json = File.ReadAllText( configFileName );

            //JToken token = null;
            //token = n2["type"];
            //if (token != null && token.Type == JTokenType.String) {
            this.actionSetsStatus.Clear();
            this.actionSetsStatusHistory.Clear();

            JToken configJsonNodes = JToken.Parse( json );

            int actionSetNum = 0;

            InputSimulator typer = new InputSimulator();
            System.Windows.Forms.Keys vKeyCode;
            JToken jsonToken = null;
            string keyString = null;
            DetectData detectorData = null;

            WalkNode( configJsonNodes, item => {
                                
                typer = new InputSimulator();
                vKeyCode = 0;
                jsonToken = null;
                keyString = null;
                detectorData = null;

                switch (item["bodyAction"].ToString()) {
                    /*/
                    case "leanLeft":
                        detectorData.bodyAction = bodyActionTypes.leanLeft;
                        detectorData = new ( JointType., JointType. ).make();
                    break;

                    case "leanRight":
                        detectorData.bodyAction = bodyActionTypes.leanRight;
                        detectorData = new ( JointType., JointType. ).make();
                    break;

                    case "leanForward":
                        detectorData.bodyAction = bodyActionTypes.leanForward;
                        detectorData = new ( JointType., JointType. ).make();
                    break;

                    case "leanBackward":
                        detectorData.bodyAction = bodyActionTypes.leanBackward;
                        detectorData = new ( JointType., JointType. ).make();
                    break;
                    /**/
                    case "turnLeft":
                        detectorData = new DetectTurn( bodyActionTypes.turnLeft, JointType.SpineShoulder, JointType.ShoulderRight, actionDetectDirections.positive, actionDetectPlanes.y ).make();
                        break;

                    case "turnRight":
                        detectorData = new DetectTurn( bodyActionTypes.turnRight, JointType.SpineShoulder, JointType.ShoulderLeft, actionDetectDirections.negative, actionDetectPlanes.y ).make();
                        break;
                    /*/
                    case "handLeftExpand":
                        detectorData.bodyAction = bodyActionTypes.handLeftExpand;
                        detectorData = new ( JointType., JointType. ).make();
                    break;
                    /**/
                    case "handLeftForward":
                        detectorData = new DetectDistance( bodyActionTypes.handLeftForward, JointType.ShoulderLeft, JointType.HandLeft, actionDetectDirections.negative, actionDetectPlanes.z ).make();
                        break;

                    case "handLeftDown":
                        detectorData = new DetectDistance( bodyActionTypes.handLeftDown, JointType.ShoulderLeft, JointType.HandLeft, actionDetectDirections.negative, actionDetectPlanes.y ).make();
                        break;

                    case "handLeftUp":
                        detectorData = new DetectDistance( bodyActionTypes.handLeftUp, JointType.ShoulderLeft, JointType.HandLeft, actionDetectDirections.positive, actionDetectPlanes.y ).make();
                        break;

                    case "handLeftOut":
                        detectorData = new DetectDistance( bodyActionTypes.handLeftOut, JointType.ShoulderLeft, JointType.HandLeft, actionDetectDirections.negative, actionDetectPlanes.x ).make();
                        break;

                    case "handLeftAcross":
                        detectorData = new DetectDistance( bodyActionTypes.handLeftAcross, JointType.ShoulderLeft, JointType.HandLeft, actionDetectDirections.positive, actionDetectPlanes.x ).make();
                        break;

                    case "handLeftBackward":
                        detectorData = new DetectDistance( bodyActionTypes.handLeftBackward, JointType.ShoulderLeft, JointType.HandLeft, actionDetectDirections.positive, actionDetectPlanes.z ).make();
                        break;
                    /*/
                    case "handRightExpand":
                        detectorData.bodyAction = bodyActionTypes.handRightExpand;
                        detectorData = new ( JointType., JointType. ).make();
                    break;
                    /**/
                    case "handRightForward":
                        detectorData = new DetectDistance( bodyActionTypes.handRightForward, JointType.ShoulderRight, JointType.HandRight, actionDetectDirections.negative, actionDetectPlanes.z ).make();
                        break;

                    case "handRightDown":
                        detectorData = new DetectDistance( bodyActionTypes.handRightDown, JointType.ShoulderRight, JointType.HandRight, actionDetectDirections.negative, actionDetectPlanes.y ).make();
                        break;

                    case "handRightUp":
                        detectorData = new DetectDistance( bodyActionTypes.handRightUp, JointType.ShoulderRight, JointType.HandRight, actionDetectDirections.positive, actionDetectPlanes.y ).make();
                        break;

                    case "handRightOut":
                        detectorData = new DetectDistance( bodyActionTypes.handRightOut, JointType.ShoulderRight, JointType.HandRight, actionDetectDirections.positive, actionDetectPlanes.x ).make();
                        break;

                    case "handRightAcross":
                        detectorData = new DetectDistance( bodyActionTypes.handRightAcross, JointType.ShoulderRight, JointType.HandRight, actionDetectDirections.negative, actionDetectPlanes.x ).make();
                        break;

                    case "handRightBackward":
                        detectorData = new DetectDistance( bodyActionTypes.handRightBackward, JointType.ShoulderRight, JointType.HandRight, actionDetectDirections.positive, actionDetectPlanes.z ).make();
                        break;

                    case "footLeftForward":
                        detectorData = new DetectDistance( bodyActionTypes.footLeftForward, JointType.HipLeft, JointType.FootLeft, actionDetectDirections.negative, actionDetectPlanes.z ).make();
                        break;

                    case "footLeftOut":
                        detectorData = new DetectDistance( bodyActionTypes.footLeftOut, JointType.HipLeft, JointType.FootLeft, actionDetectDirections.negative, actionDetectPlanes.x ).make();
                        break;

                    case "footLeftBackward":
                        detectorData = new DetectDistance( bodyActionTypes.footLeftBackward, JointType.HipLeft, JointType.FootLeft, actionDetectDirections.positive, actionDetectPlanes.z ).make();
                        break;

                    case "footLeftUp":
                        detectorData = new DetectDistance( bodyActionTypes.footLeftUp, JointType.HipLeft, JointType.FootLeft, actionDetectDirections.positive, actionDetectPlanes.y ).make();
                        break;

                    case "footLeftAcross":
                        detectorData = new DetectDistance( bodyActionTypes.footLeftAcross, JointType.HipLeft, JointType.FootLeft, actionDetectDirections.positive, actionDetectPlanes.x ).make();
                        break;

                    case "footRightForward":
                        detectorData = new DetectDistance( bodyActionTypes.footRightForward,JointType.HipRight, JointType.FootRight, actionDetectDirections.negative, actionDetectPlanes.z ).make();
                        break;

                    case "footRightOut":
                        detectorData = new DetectDistance( bodyActionTypes.footRightOut, JointType.HipRight, JointType.FootRight, actionDetectDirections.positive, actionDetectPlanes.x ).make();
                        break;

                    case "footRightBackward":
                        detectorData = new DetectDistance( bodyActionTypes.footRightBackward, JointType.HipRight, JointType.FootRight, actionDetectDirections.positive, actionDetectPlanes.z ).make();
                        break;

                    case "footRightUp":
                        detectorData = new DetectDistance( bodyActionTypes.footRightUp, JointType.HipRight, JointType.FootRight, actionDetectDirections.positive, actionDetectPlanes.y ).make();
                        break;

                    case "footRightAcross":
                        detectorData = new DetectDistance( bodyActionTypes.footRightAcross, JointType.HipRight, JointType.FootRight, actionDetectDirections.negative, actionDetectPlanes.x ).make();
                        break;

                    case "jump":
                        detectorData = new DetectJump( bodyActionTypes.jump, JointType.FootLeft, JointType.FootRight ).make();
                        break;

                    case "crouch":
                        detectorData = new DetectCrouch( bodyActionTypes.crouch, JointType.SpineShoulder, JointType.SpineBase ).make();
                        break;
                    /*/
                    case "walk":
                        detectorData.bodyAction = bodyActionTypes.walk;
                        detectorData = new ( JointType., JointType. ).make();
                    break;
                    /**/
                    default:
                        return;
                }


                jsonToken = item["distanceMax"];
                if (jsonToken != null && jsonToken.Type == JTokenType.String) {
                    // convert inches to Kinect style float of meter at millimeter level detail
                    detectorData.distanceMax = (float)( (int)jsonToken * 0.0254 );
                }
                
                jsonToken = item["distanceMin"];
                if (jsonToken != null && jsonToken.Type == JTokenType.String) {
                    // convert inches to Kinect style float of meter at millimeter level detail
                    detectorData.distanceMin = (float)( (int)jsonToken * 0.0254 ); 
                }
                
                jsonToken = item["angleMax"];
                if (jsonToken != null && jsonToken.Type == JTokenType.String) {
                    detectorData.angleMax = (int)jsonToken;
                }
                
                jsonToken = item["angleMin"];
                if (jsonToken != null && jsonToken.Type == JTokenType.String) {
                    detectorData.angleMin = (int)jsonToken;
                }


                if( detectorData.distanceMax > 0 ) {
                    if( detectorData.distanceMin > 0 ) {
                        detectorData.detectType = actionDetectTypes.distanceRange;
                    } else {
                        detectorData.detectType = actionDetectTypes.distanceMax;
                    }
                } else if( detectorData.distanceMin > 0 ) {
                    detectorData.detectType = actionDetectTypes.distanceMin;
                } else if( detectorData.angleMax > 0 ) {
                    if( detectorData.angleMin > 0 ) {
                        detectorData.detectType = actionDetectTypes.angleRange;
                    } else {
                        detectorData.detectType = actionDetectTypes.angleMax;
                    }
                } else if( detectorData.angleMin > 0 ) {
                    detectorData.detectType = actionDetectTypes.angleMin;
                }
                                
                
                jsonToken = item["keyTap"];
                if (jsonToken != null && jsonToken.Type == JTokenType.String) {
                    detectorData.executeAction = actionExecuteActions.keyTap;
                    keyString = jsonToken.ToString();
                }
                
                jsonToken = item["keyHold"];
                if (jsonToken != null && jsonToken.Type == JTokenType.String) {
                    detectorData.executeAction = actionExecuteActions.keyHold;
                    keyString = jsonToken.ToString();
                }

                switch( keyString ) {
                    case "a": vKeyCode = Keys.A; break;
                    case "b": vKeyCode = Keys.B; break;
                    case "c": vKeyCode = Keys.C; break;
                    case "d": vKeyCode = Keys.D; break;
                    case "e": vKeyCode = Keys.E; break;
                    case "f": vKeyCode = Keys.F; break;
                    case "g": vKeyCode = Keys.G; break;
                    case "h": vKeyCode = Keys.H; break;
                    case "i": vKeyCode = Keys.I; break;
                    case "j": vKeyCode = Keys.J; break;
                    case "k": vKeyCode = Keys.K; break;
                    case "l": vKeyCode = Keys.L; break;
                    case "m": vKeyCode = Keys.M; break;
                    case "n": vKeyCode = Keys.N; break;
                    case "o": vKeyCode = Keys.O; break;
                    case "p": vKeyCode = Keys.P; break;
                    case "q": vKeyCode = Keys.Q; break;
                    case "r": vKeyCode = Keys.R; break;
                    case "s": vKeyCode = Keys.S; break;
                    case "t": vKeyCode = Keys.T; break;
                    case "u": vKeyCode = Keys.U; break;
                    case "v": vKeyCode = Keys.V; break;
                    case "w": vKeyCode = Keys.W; break;
                    case "x": vKeyCode = Keys.X; break;
                    case "y": vKeyCode = Keys.Y; break;
                    case "z": vKeyCode = Keys.Z; break;
                }

                if( vKeyCode != 0 ) {
                    detectorData.vKeyCode = vKeyCode;
                }

                readyActionItems.Add( detectorData );

                this.actionSetsStatus[actionSetNum] = false;
                
                /*/
                this.sender.DogName = this.sender.DogName + "\r\n" + item["bodyAction"].ToString();
                /**/

                actionSetNum++;

            } );
        }
        
        /*/
        [DllImport("user32.dll")]
        public static extern short VkKeyScanEx(char ch, IntPtr dwhkl);
        public static void GetKeyboardShortcutForChar(char c, InputLanguage lang, out Keys key, out bool shift) {
            var keyCode = VkKeyScanEx(c, lang.Handle);
            key = (Keys) (keyCode & 0xFF);
            shift = (keyCode & 0x100) == 0x100;
        }
        /**/

        private InputSimulator typer = new InputSimulator();

        public enum actionDetectTypes {
            distanceMax,
            distanceMin,
            distanceRange,
            angleMax,
            angleMin,
            angleRange
        }

        public enum actionDetectDirections {
            negative,
            positive
        }

        public enum actionDetectPlanes {
            x,
            y,
            z
        }

        public enum actionExecuteActions {
            keyTap,
            keyHold
        }

        public enum bodyActionTypes {
            leanLeft,
            leanRight,
            leanForward,
            leanBackward,

            turnLeft,
            turnRight,

            handLeftExpand,
            handLeftForward,
            handLeftDown,
            handLeftUp,
            handLeftOut,
            handLeftAcross,
            handLeftBackward,

            handRightExpand,
            handRightForward,
            handRightDown,
            handRightUp,
            handRightOut,
            handRightAcross,
            handRightBackward,

            footLeftForward,
            footLeftOut,
            footLeftBackward,
            footLeftUp,
            footLeftAcross,

            footRightForward,
            footRightOut,
            footRightBackward,
            footRightUp,
            footRightAcross,

            jump,
            crouch,
            walk,
        }

        int processCount = 0;
        public void processConfig( BodyFrame frame, IReadOnlyDictionary<JointType, Joint> joints, Dictionary<JointType, Point> jointPoints, DrawingContext bodyDraw, DrawingContext actionHistoryDraw ) {

            this.processCount++;

            /*/
            Vector4 floor = frame.FloorClipPlane;
            CameraSpacePoint neck  = joints[JointType.SpineShoulder].Position;
            CameraSpacePoint ass   = joints[JointType.SpineBase].Position;
            CameraSpacePoint foot  = joints[JointType.FootRight].Position;

            this.sender.DogName =
                "flr x : " + floor.X + "\r\n" +
                "flr y : " + floor.Y + "\r\n" +
                "flr z : " + floor.Z + "\r\n" +
                "flr w : " + floor.W + "\r\n" +
                "ass x : " + ass.X + "\r\n" +
                "ass y : " + ass.Y + "\r\n" +
                "ass z : " + ass.Z + "\r\n" +
                "fot x : " + foot.X + "\r\n" +
                "fot y : " + foot.Y + "\r\n" +
                "fot z : " + foot.Z + "\r\n" +
                "nek x : " + neck.X + "\r\n" +
                "nek y : " + neck.Y + "\r\n" +
                "nek z : " + neck.Z + "\r\n" +

                "foot from floor : " + ((foot.X * floor.X + foot.Y * floor.Y + foot.Z * floor.Z + floor.W) / 0.0254) + "\r\n" +
                "ass from floor : " + ((ass.X * floor.X + ass.Y * floor.Y + ass.Z * floor.Z + floor.W) / 0.0254) + "\r\n" +
                "neck from floor : " + ((neck.X * floor.X + neck.Y * floor.Y + neck.Z * floor.Z + floor.W) / 0.0254) + "\r\n" +
                "ass from neck : " + ((ass.X * neck.X + ass.Y * neck.Y + ass.Z * neck.Z) / 0.0254) + "\r\n" +
                "neck from ass : " + ((neck.X * ass.X + neck.Y * ass.Y + neck.Z * ass.Z) / 0.0254) + "\r\n" +
                "neck from foot : " + ((neck.X * foot.X + neck.Y * foot.Y + neck.Z * foot.Z) / 0.0254) + "\r\n" +
                "ass from foot : " + ((foot.X * ass.X + foot.Y * ass.Y + foot.Z * ass.Z) / 0.0254) + "\r\n" +
                "\r\n"
            ;
            /**/

            bool detectStatus = false;

            int actionSetNum = -1;

            this.sender.DogName = 
                "\r\n" + "detecting : " + 
                "\r\n" + "head to left : " + ( joints[ JointType.Head ].Position.Y - joints[ JointType.FootLeft ].Position.Y ) + 
                "\r\n" + "head to right : " + ( joints[ JointType.Head ].Position.Y - joints[ JointType.FootRight ].Position.Y ) + 
                "\r\n"
            ;

            foreach ( DetectData readyAction in this.readyActionItems) {
                actionSetNum++;

                detectStatus = false;
                /*/
                this.sender.DogName = this.sender.DogName + "\r\n" + 
                    readyAction.bodyAction + " : " + 
                    readyAction.executeAction + " : " + 
                    readyAction.distanceMin + " : " + 
                    readyAction.distanceMax + " : " + 
                    readyAction.vKeyCode.ToString()
                ;
                /**/

                switch (readyAction.bodyAction) {
                    case bodyActionTypes.crouch:
                        detectStatus = new CheckCrouch( readyAction.jointBase, readyAction.jointEnd, readyAction.distanceMax ).check( frame, joints );
                    break;

                    case bodyActionTypes.jump:
                        detectStatus = new CheckJump( readyAction.jointBase, readyAction.jointEnd, readyAction.distanceMin ).check( frame, joints );
                    break;

                    default:
                        switch (readyAction.detectType) {
                            case actionDetectTypes.distanceMin:
                                switch (readyAction.direction) {
                                    case actionDetectDirections.negative:
                                        switch (readyAction.plane) {
                                            case actionDetectPlanes.x: detectStatus = new CheckNegativeDistanceMinX( readyAction.jointBase, readyAction.jointEnd, readyAction.distanceMin ).check( joints ); break;
                                            case actionDetectPlanes.y: detectStatus = new CheckNegativeDistanceMinY( readyAction.jointBase, readyAction.jointEnd, readyAction.distanceMin ).check( joints ); break;
                                            case actionDetectPlanes.z: detectStatus = new CheckNegativeDistanceMinZ( readyAction.jointBase, readyAction.jointEnd, readyAction.distanceMin ).check( joints ); break;
                                        }
                                    break;

                                    case actionDetectDirections.positive:
                                        switch (readyAction.plane) {
                                            case actionDetectPlanes.x: detectStatus = new CheckPositiveDistanceMinX( readyAction.jointBase, readyAction.jointEnd, readyAction.distanceMin ).check( joints );  break;
                                            case actionDetectPlanes.y: detectStatus = new CheckPositiveDistanceMinY( readyAction.jointBase, readyAction.jointEnd, readyAction.distanceMin ).check( joints );  break;
                                            case actionDetectPlanes.z: detectStatus = new CheckPositiveDistanceMinZ( readyAction.jointBase, readyAction.jointEnd, readyAction.distanceMin ).check( joints );  break;
                                        }
                                    break;

                                    default:
                                        return;
                                }
                            break;

                            case actionDetectTypes.angleMin:
                                switch (readyAction.direction) {
                                    case actionDetectDirections.negative:
                                        switch (readyAction.plane) {
                                            case actionDetectPlanes.y:
                                                /*/
                                                float point1a = joints[JointType.SpineShoulder].Position.Z;
                                                float point1b = joints[JointType.ShoulderRight].Position.Z;
                                                float point2a = joints[JointType.SpineShoulder].Position.X;
                                                float point2b = joints[JointType.ShoulderRight].Position.X;
                                                float angle = (float)( Math.Atan2((double)(point1a - point1b), (double)(point2a - point2b)) * 180 / Math.PI );
                                                this.sender.DogName = point1a + " > " + point1b + " : " + point2a + " > " + point2b + " : " + angle;
                                                /**/
                                                detectStatus = new CheckNegativeAngleMinY( readyAction.jointBase, readyAction.jointEnd, readyAction.angleMin ).check( joints );
                                            break;
                                        }
                                    break;

                                    case actionDetectDirections.positive:
                                        switch (readyAction.plane) {
                                            case actionDetectPlanes.y:
                                                detectStatus = new CheckPositiveAngleMinY( readyAction.jointBase, readyAction.jointEnd, 180 - readyAction.angleMin ).check( joints );
                                            break;
                                        }
                                    break;

                                    default:
                                        return;
                                }
                            break;

                            default:
                                return;
                        }
                    break;
                }

                if (detectStatus) {
                    /*/
                    this.sender.DogName = this.sender.DogName + " - ACTIVE";
                    /**/
                    switch (readyAction.executeAction) {
                        case actionExecuteActions.keyTap:
                            Keyboard.KeyPress( readyAction.vKeyCode );
                        break;
                        case actionExecuteActions.keyHold:
                            if (this.actionSetsStatus[actionSetNum] == false) {         
                                Keyboard.KeyDown( readyAction.vKeyCode );
                            }
                        break;
                    }
                    this.actionSetsStatus[actionSetNum] = true;
                } else {
                    switch (readyAction.executeAction) {
                        case actionExecuteActions.keyHold:
                            if (this.actionSetsStatus[actionSetNum] == true) {
                                Keyboard.KeyUp( readyAction.vKeyCode );
                                this.actionSetsStatus[actionSetNum] = false;
                            }
                        break;
                    }
                }
            }
            
            Pen drawPen = new Pen( Brushes.Red, 1 );

            SortedList<int, bool> statusCopy = new SortedList<int, bool>();
            foreach (int key2 in this.actionSetsStatus.Keys) {
                statusCopy[ key2 ] = this.actionSetsStatus[key2];
            }

            if (this.actionSetsStatusHistory.Count() > 500) {
                this.actionSetsStatusHistory.RemoveAt(0);
            }
            this.actionSetsStatusHistory.Add( statusCopy );

            actionHistoryDraw.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, 500, 250));

            int lineHeight = (int)( 250 / this.actionSetsStatus.Count() );

            int historyNum = 0;
            foreach ( SortedList<int, bool> historySlot in this.actionSetsStatusHistory ) {
                historyNum++;
                foreach ( KeyValuePair<int, bool> historyAction in historySlot ) {
                    if ( historyAction.Value == true ) {
                        actionHistoryDraw.DrawLine( drawPen, new Point( historyNum, historyAction.Key * lineHeight ), new Point( historyNum, historyAction.Key * lineHeight + lineHeight  ) );
                    }
                }
            }
            /**/

            /*/
            this.sender.DogName =
                readyAction.executeAction + "\r\n" +
                ( this.actionSetsStatus[actionSetNum] ? "true" : "false" ) + "\r\n" +
                joints[readyAction.jointBase].Position.Z + "\r\n" +
                ( joints[readyAction.jointBase].Position.Z * 2540 ) + "\r\n" +
                ( joints[readyAction.jointEnd].Position.Z * 2540 ) + "\r\n" +
                ((joints[readyAction.jointBase].Position.Z * 2540 ) - ( 2540 * joints[readyAction.jointEnd].Position.Z)) +  "\r\n" +
                readyAction.distanceMin.ToString() +  "\r\n" +
                detectStatus.ToString()
            ;
            /**/
        }

        public int screenSizeX = 0;
        public int screenSizeY = 0;
        private MainWindow sender;

        public UserConfig( MainWindow mainWindow ) {
            // TODO: Complete member initialization
            this.sender = mainWindow;
            this.sender.DogName = "user config";
            this.loadConfig( "sf4-new.json" );
        }

        public void actionsDetect( BodyFrame frame, IReadOnlyDictionary<JointType, Joint> joints, Dictionary<JointType, Point> jointPoints, DrawingContext bodyDraw, DrawingContext actionHistoryDraw ) {
            
            this.processConfig( frame, joints, jointPoints, bodyDraw, actionHistoryDraw );

            this.previousFrame = frame;
            this.previousJoints = joints;
            this.previousJointPoints = jointPoints;

        }

        public int getSkeletonElevation( BodyFrame frame, IReadOnlyDictionary<JointType, Joint> joints ) {
            Vector4 c = frame.FloorClipPlane;
            CameraSpacePoint j = joints[JointType.FootLeft].Position.Y < joints[JointType.FootRight].Position.Y
                ? joints[JointType.FootLeft].Position
                : joints[JointType.FootRight].Position
            ;
            return (int)( j.X * c.X + j.Y * c.Y + j.Z * c.Z + c.W );
        }
    }
}