using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ObjectListViewEx
{
    public class Song
    {
        public Song()
        {

        }

        public Song(string title, string name, int rating)
        {
            this.title = title;
            this.name = name;
            this.rating = rating;
        }

        private string title;
        private string name;
        private int rating;
        private Button btn;

        public string Title { get => title; set => title = value; }
        public string Name { get => name; set => name = value; }
        public int Rating {get => rating; set => rating = value; }
        public Button Btn { get => btn; set => btn = value; }

        public static List<Song> GetSongs()
        {
            List<Song> songs = new List<Song>();

            songs.Add(new Song() { Title = "Abc", Name = "Music", Rating = 60, Btn = new Button() { Text="ClickMe"} });
            songs.Add(new Song() { Title = "Xyz", Name = "Artis", Rating = 70, Btn = new Button() { Text = "ClickMe" } });
            songs.Add(new Song() { Title = "IJK", Name = "Song", Rating = 80, Btn = new Button() { Text = "ClickMe" } });
            songs.Add(new Song() { Title = "M", Name = "Singer", Rating = 90, Btn = new Button() { Text = "ClickMe" } });

            return songs;
        }
    }
}
