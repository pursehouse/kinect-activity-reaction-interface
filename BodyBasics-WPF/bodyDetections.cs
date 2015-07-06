using System;
using Microsoft.Kinect;
using WindowsInput;
using WindowsInput.Native;

namespace BodyBasicsWPF {
    
    using System;
    using Microsoft.Kinect;
    using System.Collections.Generic;
    using WindowsInput;
    using WindowsInput.Native;

    public class DetectData {
        
        public JointType jointBase = new JointType();
        public JointType jointEnd  = new JointType();
        
        public UserConfig.actionDetectDirections direction;
        public UserConfig.actionDetectPlanes plane;
        public UserConfig.bodyActionTypes bodyAction;
        public UserConfig.actionExecuteActions executeAction;
        public UserConfig.actionDetectTypes detectType;

        public float distanceMax;
        public float distanceMin;
        public int angleMax;
        public int angleMin;

        public List<System.Windows.Forms.Keys> vKeyCodesList;

        public DetectData( UserConfig.bodyActionTypes bodyAction, JointType BasePoint, JointType EndPoint, UserConfig.actionDetectDirections direction, UserConfig.actionDetectPlanes plane ) {
            this.jointBase  = BasePoint;
            this.jointEnd   = EndPoint;
            this.direction  = direction;
            this.plane      = plane;
            this.bodyAction = bodyAction;
        }

    }

    public class DetectBase {
        public JointType jointBase;
        public JointType jointEnd;
        public UserConfig.actionDetectDirections direction;
        public UserConfig.actionDetectPlanes plane;
        public UserConfig.bodyActionTypes bodyAction;

        public DetectBase( UserConfig.bodyActionTypes bodyAction, JointType jointBase, JointType jointEnd, UserConfig.actionDetectDirections direction, UserConfig.actionDetectPlanes plane ) {
            this.jointBase = jointBase;
            this.jointEnd = jointEnd;
            this.direction = direction;
            this.plane = plane;
            this.bodyAction = bodyAction;
        }
    
    }
    public class DetectDistance : DetectBase {
        public DetectDistance( UserConfig.bodyActionTypes bodyAction, JointType jointBase, JointType jointEnd, UserConfig.actionDetectDirections direction, UserConfig.actionDetectPlanes plane ) : base( bodyAction, jointBase, jointEnd, direction, plane ) {
        }
        public DetectData make() {
            return new DetectData( this.bodyAction, this.jointBase, this.jointEnd, this.direction, this.plane );
        }
    }
    public class DetectJump : DetectBase {
        public DetectJump( UserConfig.bodyActionTypes bodyAction, JointType jointBase, JointType jointEnd ) : base( bodyAction, jointBase, jointEnd, UserConfig.actionDetectDirections.positive, UserConfig.actionDetectPlanes.y ) {
        }
        public DetectData make() {
            return new DetectData( this.bodyAction, this.jointBase, this.jointEnd, UserConfig.actionDetectDirections.positive, UserConfig.actionDetectPlanes.y );
        }
    }

    public class DetectBothHandsUp : DetectBase
    {
        public DetectBothHandsUp(UserConfig.bodyActionTypes bodyAction, JointType jointBase, JointType jointEnd)
            : base(bodyAction, jointBase, jointEnd, UserConfig.actionDetectDirections.positive, UserConfig.actionDetectPlanes.y)
        {
        }
        public DetectData make()
        {
            return new DetectData(this.bodyAction, this.jointBase, this.jointEnd, UserConfig.actionDetectDirections.positive, UserConfig.actionDetectPlanes.y);
        }
    }

    public class DetectCrouch : DetectBase {
        public DetectCrouch( UserConfig.bodyActionTypes bodyAction, JointType jointBase, JointType jointEnd ) : base( bodyAction, jointBase, jointEnd, UserConfig.actionDetectDirections.negative, UserConfig.actionDetectPlanes.y ) {
        }
        public DetectData make() {
            return new DetectData( this.bodyAction, this.jointBase, this.jointEnd, UserConfig.actionDetectDirections.positive, UserConfig.actionDetectPlanes.y );
        }
    }  
    public class DetectTurn : DetectBase {
        public DetectTurn( UserConfig.bodyActionTypes bodyAction, JointType jointBase, JointType jointEnd, UserConfig.actionDetectDirections direction, UserConfig.actionDetectPlanes plane ) : base( bodyAction, jointBase, jointEnd, direction, plane ) {
        }
        public DetectData make() {
            return new DetectData( this.bodyAction, this.jointBase, this.jointEnd, this.direction, this.plane );
        }

    }
    
}