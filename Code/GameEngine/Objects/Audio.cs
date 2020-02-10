using Game_Engine.Managers;
using OpenTK.Audio.OpenAL;

namespace Game_Engine.Objects
{
    public class Audio
    {
        int mSource;

        public Audio(string filename, bool looping)
        {
            int buffer = ResourceManager.LoadBuffer(filename);
            //Create sounds source using the audio clip
            mSource = AL.GenSource(); //gen a Source Handle
            AL.Source(mSource, ALSourcei.Buffer, buffer); // attach the buffer to a source
            AL.Source(mSource, ALSourceb.Looping, looping); // source loops infinitely
            AL.SourceStop(mSource);
        }

        public void Delete()
        {
            AL.SourceStop(mSource);
            AL.DeleteSource(mSource);
        }

        public void Play()
        {
            if (AL.GetSourceState(mSource) == ALSourceState.Initial ||
                AL.GetSourceState(mSource) == ALSourceState.Stopped)
            {
                AL.SourcePlay(mSource);
            }
        }

        public int Source
        {
            get { return mSource; }
        }        
    }
}
