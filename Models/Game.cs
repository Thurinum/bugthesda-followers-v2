using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace SessionProject2W5.Models
{
    public class Game
    {
        public int Id;               // numeric ID (PK)
        public string Name;          // display name
        public string ShortName;     // spaceless name used to create image paths (e.g. {shortname}_logo.png)
        public string Description;   // description
        public string Tagline;       // game's tagline (slogan)
        public int    YearReleased;  // game's tagline (slogan)
        public string Director;      // whoever directed the game (probably Todd Howard)
        public string Color;         // theme color for buttons, inputs, etc.

        public List<string> Facts;         // facts about the game
        public List<Follower> Followers;   // children

        public SharedInfo SharedInfo;

        public Game()
        {
            Facts = new List<string>();    
            Followers = new List<Follower>();
        }
    }
}