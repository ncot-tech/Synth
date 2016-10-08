using Microsoft.Xna.Framework;
using System;

namespace Synthesizer
{
    public static class Oscillator
    {
        public static float Sine(float frequency, ref double time, int sampleRate = 44100)
        {
            return (float)Math.Sin(time += (frequency * 2 * Math.PI) / sampleRate);
        }

        public static float Square(float frequency, ref double time, int sampleRate = 44100)
        {
            return Sine(frequency, ref time) >= 0 ? 1.0f : -1.0f;
        }

        public static float Sawtooth(float frequency, ref double time, int sampleRate = 44100)
        {
            return (float)(2 * (time * frequency - Math.Floor(time * frequency + 0.5)));
        }

        public static float Triangle(float frequency, ref double time, int sampleRate = 44100)
        {
            return Math.Abs(Sawtooth(frequency, ref time)) * 2.0f - 1.0f;
        }

        public static float Moag(float frequency, ref double time, int sampleRate = 44100)
        {
            return MathHelper.SmoothStep(Sine(frequency, ref time), Square(frequency, ref time), (float)time);
        }
    }

    public delegate float OscillatorDelegate(float frequency, ref double time, int sampleRate = 44100);
}
