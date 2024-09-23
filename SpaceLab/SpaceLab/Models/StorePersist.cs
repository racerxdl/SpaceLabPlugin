using Torch;
using VRage.Collections;

namespace SpaceLab.Models
{
    public class StorePersist: ViewModel
    {

        private ObservableCollection<Player> players = new ObservableCollection<Player>();

        public ObservableCollection<Player> Players
        {
            get => players;
            set => SetValue(ref players, value);
        }

        public void AddPlayer(Player player)
        {
            foreach (var p in players)
            {
                if (p.Id == player.Id)
                {
                    players.Remove(p);
                    break;
                }
            }

            players.Add(player);
            OnPropertyChanged(nameof(Players));
        }

        public void RemovePlayer(string id)
        {
            foreach (var p in players)
            {
                if (p.Id == id)
                {
                    players.Remove(p);
                    OnPropertyChanged(nameof(Players));
                    break;
                }
            }
        }

        public void UpdatePlayer(Player player)
        {
            foreach (var p in players)
            {
                if (p.Id == player.Id)
                {
                    p.Name = player.Name;
                    p.Faction = player.Faction;
                    p.SteamId = player.SteamId;
                    p.IsOnline = player.IsOnline;
                    p.Deaths = player.Deaths;
                    p.Position = player.Position;
                    p.Rotation = player.Rotation;
                    OnPropertyChanged(nameof(Players));
                    break;
                }
            }   
        }
    }
}
