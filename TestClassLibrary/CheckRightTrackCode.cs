using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit;
using NUnit.Framework;

using RussianPostClassLibrary.ValidationCheck;

namespace TestClassLibrary
{
    [TestFixture]
    public class CheckRightTrackCode//Тестовый класс для проверки правильности функционирования обработки поля для ввода трек-кода
    {
        [Test()]
        public void ExeptionEmptyString()
        {
            bool actual;
            Assert.That(() => actual = TrackCodeCheck.CheckTrackCode("", true, false), Throws.TypeOf<ArgumentException>());
        }
        [Test()]
        public void ExeptionNullString()
        {
            bool actual;
            Assert.That(() => actual = TrackCodeCheck.CheckTrackCode(null, true, false), Throws.TypeOf<ArgumentException>());
        }
        [Test()]
        public void ExeptionOverflowString()
        {
            bool actual;
            Assert.That(() => actual = TrackCodeCheck.CheckTrackCode(new String('R',131), true, false), Throws.TypeOf<ArgumentException>());
        }
        [Test()]
        public void ExeptionIncorrectString()
        {
            bool actual;
            Assert.That(() => actual = TrackCodeCheck.CheckTrackCode("R{123456789CN", true, false), Throws.TypeOf<ArgumentException>());
        }
        [Test()]
        public void ExeptionIncorrectLengthStringInternationalTrack()
        {
            bool actual;
            Assert.That(() => actual = TrackCodeCheck.CheckTrackCode("RN1234567899CN", true, false), Throws.TypeOf<ArgumentException>());
        }
        //[Test()]
        //public void ExeptionIncorrectRepresentationStringInternationalTrack1()
        //{
        //    bool actual;
        //    Assert.That(() => actual = TrackCodeCheck.CheckTrackCode("QN123456789CN", true, false), Throws.TypeOf<ArgumentException>());
        //}
        [TestCase("QN123456789CN")]
        [TestCase("RN1234Q6789CN")]
        public void ExeptionIncorrectRepresentationStringInternationalTrack(string trackCode)
        {
            bool actual;
            Assert.That(() => actual = TrackCodeCheck.CheckTrackCode(trackCode, true, false), Throws.TypeOf<ArgumentException>());
        }
        [Test()]
        public void ExeptionIncorrectLengthStringLocalTrack()
        {
            bool actual;
            Assert.That(() => actual = TrackCodeCheck.CheckTrackCode("123456789012345", true, false), Throws.TypeOf<ArgumentException>());
        }
        [Test()]
        public void ExeptionIncorrectRepresentationStringLocalTrack()
        {
            bool actual;
            Assert.That(() => actual = TrackCodeCheck.CheckTrackCode("123456F8901234", true, false), Throws.TypeOf<ArgumentException>());
        }
        [Test()]
        public void ExeptionIncorrectSymbolInTrack()
        {
            bool actual;
            Assert.That(() => actual = TrackCodeCheck.CheckTrackCode("1=3456F8901234", true, false), Throws.TypeOf<ArgumentException>());
        }
        [TestCase("A",true)]
        [TestCase("Z", true)]
        [TestCase("G", true)]
        [TestCase("0", true)]
        [TestCase("5", true)]
        [TestCase("9", true)]
        [TestCase("+", true)]
        [TestCase("|", true)]
        public void CorrectOneSymbol(string symbol,bool expected)
        {
            bool actual = TrackCodeCheck.CheckTrackCode(symbol, false, false);
            Assert.AreEqual(expected, actual);
        }
        [TestCase("/", false)]
        [TestCase(":", false)]
        [TestCase("@", false)]
        [TestCase("[", false)]
        public void IncorrectSymbol(string symbol, bool expected)
        {
            bool actual;
            Assert.That(() => actual = TrackCodeCheck.CheckTrackCode(symbol, false, false), Throws.TypeOf<ArgumentException>());
        }
        [TestCase("RU123456789CN",true)]
        [TestCase("12345678912345", true)]
        public void NormalTrackCode(string trackCode, bool expected)
        {
            bool actual = TrackCodeCheck.CheckTrackCode(trackCode,true,false);
            Assert.AreEqual(expected, actual);
        }

        
    }
}
