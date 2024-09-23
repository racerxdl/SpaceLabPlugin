using VRageMath;

namespace SpaceLab.Models
{
    public class Player
    {
        public long IdentityId => long.Parse(Id);
        public string Id { get; set; }
        public string Name { get; set; }
        public string Faction { get; set; }
        public string SteamId { get; set; }
        public bool IsOnline { get; set; }
        public int Deaths { get; set; }
        public Vector3D Position { get; set; }
        public Quaternion Rotation { get; set; }
        public double X { get { return Position.X; } }
        public double Y { get { return Position.Y; } }
        public double Z { get { return Position.Z; } }
    }
}
