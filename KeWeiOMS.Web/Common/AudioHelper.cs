using System;
using System.Collections.Generic;
using System.Text;
using System.Media;
using System.Reflection;
using System.IO;
using System.Net;

namespace KeWeiOMS.Web
{
    public class AudioHelper
    {

        string path;

        public string Path
        {
            get
            {
                return path;
            }
            set { path = value; }
        }
        private static AudioHelper instance;
        public static AudioHelper Instance
        {
            get
            {
                lock (typeof(AudioHelper))
                {
                    if (instance == null)
                        instance = new AudioHelper();
                    return instance;
                }
            }
        }




        public void PlayDingDong()
        {
            SoundPlayer player = new SoundPlayer();
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://122.227.207.204/audio/security.wav");
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            player.Stream = receiveStream;
            player.Play();
            myHttpWebResponse.Close();
            receiveStream.Close();
           
        }
        public void PlayRing()
        {
            SoundPlayer player = new SoundPlayer();
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://122.227.207.204/audio/ring.wav");
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            player.Stream = receiveStream;
            player.Play();
            myHttpWebResponse.Close();
            receiveStream.Close();
            //SoundPlayer p = new SoundPlayer(Path + "/Audio/" + "ring.wav");
            //p.Play();
        }


        public void PlayWav(string url)
        {
            SoundPlayer player = new SoundPlayer();
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(@"url");
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            player.Stream = receiveStream;
            player.Play();
            myHttpWebResponse.Close();
            receiveStream.Close();
        }


    }
}
