using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    class ChordBuilder
    {
        private class ChordNote
        {
            public Note note;
            public IntervalQuality thirdToNextNote;
            public List<Note> octaves;

            public ChordNote(Note n, IntervalQuality quality)
            {
                note = n;
                quality = thirdToNextNote;
                octaves = new List<Note>();
            }
        }

        private List<ChordNote> chord;
        private List<Note> orphanNotes;

        public ChordBuilder()
        {
            chord = new List<ChordNote>();
            orphanNotes = new List<Note>();
        }

        public void AddNote(Note noteToAdd)
        {
            orphanNotes.Add(noteToAdd);

            MergeOrphanNotes();
        }

        private void MergeOrphanNotes()
        {
            if (chord.Count == 0 && orphanNotes.Count != 0)
            {
                chord.Add(new ChordNote(orphanNotes[0], IntervalQuality.Undefined));
                orphanNotes.RemoveAt(0);
            }

            bool chordHasBeenExtended;

            do
            {
                chordHasBeenExtended = false;

                // Start from the back so we check the most recently added first
                for (int i = orphanNotes.Count - 1; i >= 0; i--)
                {
                    // Check to see if this note matches any others already in the chord
                    if (TryMergeNoteIntoExistingChordNotes(orphanNotes[i]))
                    {
                        orphanNotes.RemoveAt(i);
                        continue;
                    }

                    // Try to append this note to the beginning or end of the chord
                    IntervalQuality quality = IntervalQuality.Undefined;
                    if((quality = IsUninvertedMajorOrMinorThird(orphanNotes[i], chord[0].note)) != IntervalQuality.Undefined)
                    {
                        // Prepend to the beginning of the chord
                        ChordNote toPrepend = new ChordNote(orphanNotes[i], quality);
                        chord.Insert(0, toPrepend);
                        orphanNotes.RemoveAt(i);
                        continue;
                    }
                    
                    if ((quality = IsUninvertedMajorOrMinorThird(chord[chord.Count - 1].note, orphanNotes[i])) != IntervalQuality.Undefined)
                    {
                        // Append to the end of the chord
                        ChordNote toAppend = new ChordNote(orphanNotes[i], IntervalQuality.Undefined);
                        chord[chord.Count - 1].thirdToNextNote = quality;
                        chord.Add(toAppend);
                        orphanNotes.RemoveAt(i);
                    }
                }
            } while (chordHasBeenExtended);
        }

        private IntervalQuality IsUninvertedMajorOrMinorThird(Note n1, Note n2)
        {
            Interval interval = new Interval(n1, n2);
            IntervalMatch match = interval.CanBeCalled(IntervalQuality.Major, 3);
            if (match == IntervalMatch.ExactMatch || match == IntervalMatch.UninvertedMatch) return IntervalQuality.Major;

            match = interval.CanBeCalled(IntervalQuality.Minor, 3);
            if (match == IntervalMatch.ExactMatch || match == IntervalMatch.UninvertedMatch) return IntervalQuality.Minor;

            return IntervalQuality.Undefined;
        }

        private bool TryMergeNoteIntoExistingChordNotes(Note n)
        {
            foreach (ChordNote chordNote in chord)
            {
                if (chordNote.note.NoteName == n.NoteName)
                {
                    chordNote.octaves.Add(n);
                    return true;
                }
            }

            return false;
        }
    }

    public class Chord
    {

    }
}
