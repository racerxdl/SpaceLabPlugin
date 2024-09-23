using System;
using System.Collections.ObjectModel;
using Sandbox.Game.Screens.Helpers;
using Torch;
using VRage.Game.ModAPI;
using VRageMath;

namespace SpaceLab
{
    public class GlobalGpsConfig_GPS : ViewModel
    {
        private string _createdBy;
        private Guid _id = Guid.NewGuid();
        private string _name;
        private double _x;
        private double _y;
        private double _z;

        public GlobalGpsConfig_GPS() { }
        public GlobalGpsConfig_GPS(Vector3D axis, string name, string createdBy)
        {
            X = axis.X;
            Y = axis.Y;
            Z = axis.Z;
            CreatedBy = createdBy;
            Name = name;
        }
        
        public double X
        {
            get => _x;
            set => SetValue(ref _x, value); 
        }
        
        public double Y
        {
            get => _y;
            set => SetValue(ref _y, value); 
        }
        
        public double Z
        {
            get => _z;
            set => SetValue(ref _z, value); 
        }

        public string CreatedBy
        {
            get => _createdBy;
            set => SetValue(ref _createdBy, value);
        }

        public Guid Id
        {
            get => _id;
            set => SetValue(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
        
        public MyGps ToMyGps()
        {
            var gps = new MyGps();
            gps.Coords = new Vector3D(_x, _y, _z);
            gps.Name = _name;
            gps.GPSColor = Color.Gold;
            gps.AlwaysVisible = true;
            gps.ShowOnHud = true;
            gps.DiscardAt = null;
            gps.UpdateHash();
            
            return gps;
        }
    }
        
    public class GlobalGpsConfig : ViewModel
    {

        private MyPromoteLevel _promoteLevel = MyPromoteLevel.Moderator;
        public MyPromoteLevel UsagePromoteLevel
        {
            get => _promoteLevel;
            set => SetValue(ref _promoteLevel, value);
        }

        private MyPromoteLevel _manageLevel = MyPromoteLevel.Admin;
        public MyPromoteLevel ManagePromoteLevel
        {
            get => _manageLevel;
            set => SetValue(ref _manageLevel, value);
        }

        private ObservableCollection<GlobalGpsConfig_GPS> _positions = new ObservableCollection<GlobalGpsConfig_GPS>();
        public ObservableCollection<GlobalGpsConfig_GPS> GlobalSavedPositions { get => _positions; set => SetValue(ref _positions, value); }
    }
}