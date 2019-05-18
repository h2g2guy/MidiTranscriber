using System;

namespace Engine
{
    public enum NoteName
    {
        C = 0,
        Db = 1,
        D = 2,
        Eb = 3,
        E = 4,
        F = 5,
        Gb = 6,
        G = 7,
        Ab = 8,
        A = 9,
        Bb = 10,
        B = 11
    }

    public enum IntervalQuality
    {
        Undefined = -1,
        Diminished = 0,
        Minor = 1,
        Perfect = 2,
        Major = 3,
        Augmented = 4
    }

    public enum IntervalMatch
    {
        NoMatch = 0,
        ExactMatch = 1,
        UninvertedMatch = 2,
        InvertedMatch = 3
    }

    public class InvalidIntervalException : Exception { }

    public class Interval
    {
        private Note note1, note2;

        public Interval(Note n1, Note n2)
        {
            note1 = n1;
            note2 = n2;
        }

        public IntervalMatch CanBeCalled(IntervalQuality quality, int size)
        {
            int targetSemitones = GetSemitonesInInterval(quality, size);
            int thisIntervalSemitones = Math.Abs(note1 - note2);
            if (targetSemitones == thisIntervalSemitones) return IntervalMatch.ExactMatch;

            // not an exact match; reduce both intervals to less than an octave
            targetSemitones = targetSemitones % 12;
            thisIntervalSemitones = thisIntervalSemitones % 12;
            if (targetSemitones == thisIntervalSemitones) return IntervalMatch.UninvertedMatch;

            // still not a match; invert one of the intervals as a last try
            thisIntervalSemitones = 12 - thisIntervalSemitones;
            if (targetSemitones == thisIntervalSemitones) return IntervalMatch.InvertedMatch;

            return IntervalMatch.NoMatch;
        }

        public static int GetSemitonesInInterval(IntervalQuality quality, int size)
        {
            int zeroBasedSize = size - 1;
            int octaveOffset = (zeroBasedSize / 7) * 12;
            int baseInterval = zeroBasedSize % 7;
            int baseIntervalSize = intervalTable[baseInterval, (int)quality];

            if (baseIntervalSize == -9) throw new InvalidIntervalException();

            return octaveOffset + baseIntervalSize;
        }

        private static int[,] intervalTable = new int[,]
        {
            /* unison */ { -1, -9, 0, -9, 1 },
            /* second */ { 0, 1, -9, 2, 3 },
            /* third */ { 2, 3, -9, 4, 5 },
            /* fourth */ { 4, -9, 5, -9, 6 },
            /* fifth */ { 6, -9, 7, -9, 8 },
            /* sixth */ { 7, 8, -9, 9, 10 },
            /* seventh */ { 9, 10, -9, 11, 12 },
        };
    }

    public class Note
    {
        private byte midiNote;

        public Note(byte midiNote)
        {
            this.midiNote = midiNote;
        }

        public NoteName NoteName => this;

        public static implicit operator NoteName(Note note)
        {
            return (NoteName)(note.midiNote % 12);
        }

        public static int operator- (Note n1, Note n2)
        {
            return Math.Abs(n1.midiNote - n2.midiNote);
        }
    }
}
