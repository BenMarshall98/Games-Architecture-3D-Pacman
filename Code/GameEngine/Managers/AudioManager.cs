using Game_Engine.Camera;
using Game_Engine.Objects;
using OpenTK;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Engine.Managers
{
    public class Buffer
    {
        public int buffer;
        public bool rewind;
    }
    public class AudioManager
    {
        Dictionary<string, Buffer> buffers = new Dictionary<string, Buffer>();
        Dictionary<string, int> rewindSources = new Dictionary<string, int>();
        List<int> sources = new List<int>();
        AudioContext audioContext;
        aCamera camera;

        static private AudioManager instance;

        static public AudioManager Instance()
        {
            if (instance == null)
            {
                instance = new AudioManager();
            }
            return instance;
        }

        private AudioManager()
        {
            audioContext = new AudioContext();
            Reset();
        }

        public void Reset()
        {
            buffers = new Dictionary<string, Buffer>();
        }

        public void Delete(bool partialDelete = false)
        {
            foreach (int source in rewindSources.Values)
            {
                AL.SourceStop(source);
                AL.DeleteSource(source);
            }

            for (int i = 0; i < sources.Count; i++)
            {
                AL.SourceStop(sources[i]);
                AL.DeleteSource(sources[i]);
            }
            if (!partialDelete)
            {
                audioContext.Dispose();
            }
        }

        public void SetCamera(aCamera pCamera)
        {
            camera = pCamera;
        }

        public void AddAudio(string audioName, string fileName, bool rewind)
        {
            Buffer buffer;
            buffers.TryGetValue(audioName, out buffer);
            if (buffer == null)
            {
                buffer = new Buffer();
                buffer.buffer = ResourceManager.LoadAudio(fileName);
                buffer.rewind = rewind;
                buffers.Add(audioName, buffer);
            }
            
        }

        public void Update()
        {
            if (camera == null)
            {
                Console.WriteLine("Need to setup the camera before trying to update");
                return;
            }
            Vector3 position = camera.Position;
            Vector3 direction = camera.Direction;
            Vector3 up = camera.Up;
            for (int i = 0; i < rewindSources.Count; i++)
            {
                var item = rewindSources.ElementAt(i);
                if (AL.GetSourceState(item.Value) == ALSourceState.Stopped)
                {
                    AL.SourceStop(item.Value);
                    AL.DeleteSource(item.Value);
                    rewindSources.Remove(item.Key);
                    i--;
                }
                else
                {
                    AL.Source(item.Value, ALSource3f.Position, ref position);
                }
            }

            for (int i = 0; i < sources.Count; i++)
            {
                if (AL.GetSourceState(sources[i]) == ALSourceState.Stopped)
                {
                    AL.SourceStop(sources[i]);
                    AL.DeleteSource(sources[i]);
                    sources.RemoveAt(i);
                    i--;
                }
                else
                {
                    AL.Source(sources[i], ALSource3f.Position, ref position);
                }
            }
            AL.Listener(ALListener3f.Position, ref position);
            AL.Listener(ALListenerfv.Orientation, ref direction, ref up);
        }

        public void PlayAudio(string audioName)
        {
            Buffer buffer;
            buffers.TryGetValue(audioName, out buffer);
            if (buffer != null)
            {
                if (!buffer.rewind)
                {
                    //Create sounds source using the audio clip
                    int mSource = AL.GenSource(); //gen a Source Handle
                    AL.Source(mSource, ALSourcei.Buffer, buffer.buffer); // attach the buffer to a source
                    AL.Source(mSource, ALSourceb.Looping, false); // source loop
                    AL.SourcePlay(mSource);
                    sources.Add(mSource);
                }
                else
                {
                    int source;
                    rewindSources.TryGetValue(audioName, out source);

                    if (source == 0)
                    {
                        source = AL.GenSource(); //gen a Source Handle
                        AL.Source(source, ALSourcei.Buffer, buffer.buffer); // attach the buffer to a source
                        AL.Source(source, ALSourceb.Looping, false); // source loop
                        rewindSources.Add(audioName, source);
                    }
                    AL.SourcePlay(source);
                }
            }
        }

        public void PlayAudio(Audio audio, Vector3 position)
        {
            audio.Play();
            int source = audio.Source;
            AL.Source(source, ALSource3f.Position, ref position);
        }
    }
}
