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

    public class getInfo {
        public CameraSpacePoint lowestFoot( BodyFrame frame, IReadOnlyDictionary<JointType, Joint> joints ) {
	        return joints[ JointType.FootLeft ].Position.Y < joints[ JointType.FootRight ].Position.Y
		        ? joints[ JointType.FootLeft ].Position
		        : joints[ JointType.FootRight ].Position
	        ;
        }
        public float elevation( BodyFrame frame, IReadOnlyDictionary<JointType, Joint> joints ) {
            CameraSpacePoint foot = this.lowestFoot( frame, joints );
	        Vector4 floor = frame.FloorClipPlane;
            return (float)( foot.X * floor.X + foot.Y * floor.Y + foot.Z * floor.Z + floor.W );
        }
    }

    public class CheckDistanceBase {  
        public JointType jointBase = new JointType();
        public JointType jointEnd  = new JointType();
    }
    
    public class CheckDistanceMinBase : CheckDistanceBase {
        public float distanceMin = 0;
        public CheckDistanceMinBase( JointType jointBase, JointType jointEnd, float distanceMin ) {
		    this.jointBase = jointBase; 
            this.jointEnd = jointEnd;
            this.distanceMin = distanceMin;
        } 
    }
    public class CheckDistanceMaxBase : CheckDistanceBase {
        public float distanceMax = 0;
        public CheckDistanceMaxBase( JointType jointBase, JointType jointEnd, float distanceMax ) {
		    this.jointBase = jointBase; 
            this.jointEnd = jointEnd;
            this.distanceMax = distanceMax;
        } 
    }

    public class CheckNegativeDistanceMinX : CheckDistanceMinBase {
        public CheckNegativeDistanceMinX( JointType jointBase, JointType jointEnd, float distanceMin ) : base( jointBase, jointEnd, distanceMin ) {
        }
        public bool check( IReadOnlyDictionary<JointType, Joint> joints ) {
            return ( joints[this.jointBase].Position.X - joints[this.jointEnd].Position.X ) > this.distanceMin;
        }
    }

    public class CheckPositiveDistanceMinX : CheckDistanceMinBase {
        public CheckPositiveDistanceMinX( JointType jointBase, JointType jointEnd, float distanceMin ) : base( jointBase, jointEnd, distanceMin ) {
        }
        public bool check( IReadOnlyDictionary<JointType, Joint> joints ) {
            return ( joints[this.jointEnd].Position.X - joints[this.jointBase].Position.X ) > this.distanceMin;
        }
    }

    public class CheckNegativeDistanceMinY : CheckDistanceMinBase {
        public CheckNegativeDistanceMinY( JointType jointBase, JointType jointEnd, float distanceMin ) : base( jointBase, jointEnd, distanceMin ) {
        }
        public bool check( IReadOnlyDictionary<JointType, Joint> joints ) {
            return ( joints[this.jointBase].Position.Y - joints[this.jointEnd].Position.Y ) > this.distanceMin;
        }
    }

    public class CheckPositiveDistanceMinY : CheckDistanceMinBase {
        public CheckPositiveDistanceMinY( JointType jointBase, JointType jointEnd, float distanceMin ) : base( jointBase, jointEnd, distanceMin ) {
        }
        public bool check( IReadOnlyDictionary<JointType, Joint> joints ) {
            return ( joints[this.jointEnd].Position.Y - joints[this.jointBase].Position.Y ) > this.distanceMin;
        }
    }

    public class CheckNegativeDistanceMinZ : CheckDistanceMinBase {
        public CheckNegativeDistanceMinZ( JointType jointBase, JointType jointEnd, float distanceMin ) : base( jointBase, jointEnd, distanceMin ) {
        }
        public bool check( IReadOnlyDictionary<JointType, Joint> joints ) {
            return ( joints[this.jointBase].Position.Z - joints[this.jointEnd].Position.Z ) > this.distanceMin;
        }
    }

    public class CheckPositiveDistanceMinZ : CheckDistanceMinBase {
        public CheckPositiveDistanceMinZ( JointType jointBase, JointType jointEnd, float distanceMin ) : base( jointBase, jointEnd, distanceMin ) {
        }
        public bool check( IReadOnlyDictionary<JointType, Joint> joints ) {
            return ( joints[this.jointEnd].Position.Z - joints[this.jointBase].Position.Z ) > this.distanceMin;
        }
    }
    
    public class CheckAngleBase {  
        public JointType jointBase = new JointType();
        public JointType jointEnd  = new JointType();
    }

    public class CheckAngleMinBase : CheckAngleBase {
        public int angleMin = 0;
        public CheckAngleMinBase( JointType jointBase, JointType jointEnd, int angleMin ) {
		    this.jointBase = jointBase; 
            this.jointEnd = jointEnd;
            this.angleMin = angleMin;
        } 
    }
    public class CheckPositiveAngleMinY : CheckAngleMinBase {
        public CheckPositiveAngleMinY( JointType jointBase, JointType jointEnd, int angleMin ) : base( jointBase, jointEnd, angleMin ) {
        }
        public bool check( IReadOnlyDictionary<JointType, Joint> joints ) {
            float baseZ = joints[this.jointBase].Position.Z; // spine
            float endZ  = joints[this.jointEnd].Position.Z;  // shoulder
            float baseX = joints[this.jointBase].Position.X; // spine
            float endX  = joints[this.jointEnd].Position.X;  // shoulder
            // check if shoulder is closer Z distance from camera than spine
	        if( baseZ > endZ ) {
                // check if shoulder is the correct direction from spine to check if person is facing forwards
		        if( baseX < endX ) {
                    return ( Math.Atan2( baseZ - endZ, baseX - endX ) * 180 / Math.PI ) < this.angleMin;
		        }
            }
            return false;
        }
    }
    public class CheckNegativeAngleMinY : CheckAngleMinBase {
        public CheckNegativeAngleMinY( JointType jointBase, JointType jointEnd, int angleMin ) : base( jointBase, jointEnd, angleMin ) {
        }
        public bool check( IReadOnlyDictionary<JointType, Joint> joints ) {
            float baseZ = joints[this.jointBase].Position.Z; // spine
            float endZ  = joints[this.jointEnd].Position.Z;  // shoulder
            float baseX = joints[this.jointBase].Position.X; // spine
            float endX  = joints[this.jointEnd].Position.X;  // shoulder
	        // check if shoulder is closer Z distance from camera than spine
            if( baseZ > endZ ) {
                // check if shoulder is the correct direction from spine to check if person is facing forwards
		        if( baseX > endX ) {
                    return ( Math.Atan2( baseZ - endZ, baseX - endX ) * 180 / Math.PI ) > this.angleMin;
		        }
            }
            //typer.Keyboard.TextEntry( "\n" );
            return false;
        }
    }
    public class CheckJump : CheckDistanceMinBase {
        public CheckJump( JointType jointBase, JointType jointEnd, float distanceMin ) : base( jointBase, jointEnd, distanceMin ) {
        }
        public bool check( BodyFrame frame, IReadOnlyDictionary<JointType, Joint> joints ) {
            getInfo info = new getInfo();
            return info.elevation( frame, joints ) > this.distanceMin;
        }
    }
    public class CheckBothHandsUp : CheckDistanceMinBase
    {
        public CheckBothHandsUp(JointType jointBase, JointType jointEnd, float distanceMin)
            : base(jointBase, jointEnd, distanceMin)
        {
        }
        public bool check(BodyFrame frame, IReadOnlyDictionary<JointType, Joint> joints)
        {
            getInfo info = new getInfo();
            return joints[JointType.HandLeft].Position.Y - joints[JointType.Head].Position.Y > this.distanceMin &&
            joints[JointType.HandRight].Position.Y - joints[JointType.Head].Position.Y > this.distanceMin;
        }
    }
    public class CheckCrouch : CheckDistanceMaxBase {
        public CheckCrouch( JointType jointBase, JointType jointEnd, float distanceMax ) : base( jointBase, jointEnd, distanceMax ) {
        }
        public bool check( BodyFrame frame, IReadOnlyDictionary<JointType, Joint> joints ) {

            getInfo c = new getInfo();
            float elevation = c.elevation( frame, joints );

			if( elevation < 10  ) {
                CameraSpacePoint foot = c.lowestFoot( frame, joints );
				double curHeight = joints[ JointType.Head ].Position.Y - foot.Y;
				if( this.distanceMax > curHeight ) {
                    return true;
				}
			}

            return false;


            /*/
            CameraSpacePoint neck  = joints[JointType.SpineShoulder].Position;
            CameraSpacePoint ass   = joints[JointType.SpineBase].Position;
            CameraSpacePoint foot  = joints[JointType.FootRight].Position;
	        Vector4 floor = frame.FloorClipPlane;

            float deltaX = neck.X - ass.X;
            float deltaY = neck.Y - ass.Y;
            float deltaZ = neck.Z - ass.Z;
            
            float torsoeLength = (float) Math.Sqrt( deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ );

            deltaX = neck.X - foot.X;
            deltaY = neck.Y - foot.Y;
            deltaZ = neck.Z - foot.Z;
            
            float neckHeight    = (float) Math.Sqrt( deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ );
            
            
            deltaX = ass.X - foot.X;
            deltaY = ass.Y - foot.Y;
            deltaZ = ass.Z - foot.Z;

            float assHeight    = (float) Math.Sqrt( deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ );
            

            InputSimulator typer = new InputSimulator();
            
            float torsoeLength2  = (float)((neck.X * ass.X + neck.Y * ass.Y + neck.Z * ass.Z ) );
            float assHeight2     = (float)((ass.X * foot.X + ass.Y * foot.Y + ass.Z * foot.Z ) );
            float neckHeight2    = (float)((neck.X * foot.X + neck.Y * foot.Y + neck.Z * foot.Z ) );
            
            return (torsoeLength * 2) > neckHeight;
            /**/
            /*/
	        Vector4 c = frame.FloorClipPlane;
	        CameraSpacePoint j = joints[ JointType.FootLeft ].Position.Y < joints[ JointType.FootRight ].Position.Y
		        ? joints[ JointType.FootLeft ].Position
		        : joints[ JointType.FootRight ].Position
	        ;
            return ((j.X * c.X + j.Y * c.Y + j.Z * c.Z + c.W) / 0.0254) > 1;
            /**/


        }    
    }
    
}