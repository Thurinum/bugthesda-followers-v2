﻿using System.Collections.Generic;

namespace SessionProject2W5.Models
{
    public class SharedInfo
    {
        public List<Race>  Races;
        public List<Class> Classes;

        public SharedInfo()
        {
            Races = new List<Race>();
            Classes = new List<Class>();
        }
    }
}