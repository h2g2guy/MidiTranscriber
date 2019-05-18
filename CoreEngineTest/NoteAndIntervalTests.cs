using System;
using Xunit;
using Engine;

namespace CoreEngineTest
{
    public class NoteAndIntervalTests
    {
        [Fact]
        public void NoteNameAndCanBeCalledTest()
        {
            Note c = new Note(0);
            Assert.Equal(NoteName.C, c);
            Note f = new Note(5);
            Assert.Equal(NoteName.F, f);
            Note f2 = new Note(17);
            Assert.Equal(NoteName.F, f2);
            Note g2 = new Note(19);
            Assert.Equal(NoteName.G, g2);

            Interval cToF = new Interval(c, f);
            Assert.Equal(IntervalMatch.ExactMatch, cToF.CanBeCalled(IntervalQuality.Perfect, 4));
            Assert.Equal(IntervalMatch.InvertedMatch, cToF.CanBeCalled(IntervalQuality.Perfect, 5));
            Assert.Equal(IntervalMatch.NoMatch, cToF.CanBeCalled(IntervalQuality.Major, 6));

            Interval cToF2 = new Interval(c, f2);
            Assert.Equal(IntervalMatch.ExactMatch, cToF2.CanBeCalled(IntervalQuality.Perfect, 11));
            Assert.Equal(IntervalMatch.UninvertedMatch, cToF2.CanBeCalled(IntervalQuality.Perfect, 4));
            Assert.Equal(IntervalMatch.InvertedMatch, cToF2.CanBeCalled(IntervalQuality.Perfect, 5));

            Interval f2ToG2 = new Interval(f2, g2);
            Assert.Equal(IntervalMatch.ExactMatch, f2ToG2.CanBeCalled(IntervalQuality.Major, 2));
            Assert.Equal(IntervalMatch.InvertedMatch, f2ToG2.CanBeCalled(IntervalQuality.Minor, 7));
        }
    }
}
