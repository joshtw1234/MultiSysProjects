//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   PC_VolumeControl Version 1.0.0.0
//   A class library for creating a mixer control and controlling the volume on your computer
//
//   Copyright (C) 2007  
//   Richard L. McCutchen 
//   Email: psychocoder@dreamincode.net
//   Created: 04OCT06
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <http://www.gnu.org/licenses/>.
//*****************************************************************************************

namespace PCVolumeControl.Constants
{
    /// <summary>
    /// Class to hold all the constants needed for controlling the system sound
    /// </summary>
    public static class VolumeConstants
    {
        public const int MIXER_LONG_NAME_CHARS = 64;
        public const int MIXER_SHORT_NAME_CHARS = 16;
        public const int MAXPNAMELEN = 32;
#if false
        public const int MMSYSERR_NOERROR = 0;
        public const int MAXPNAMELEN = 32;
        
        public const int MIXER_GETLINEINFOF_COMPONENTTYPE = 0x3;
        public const int MIXER_GETCONTROLDETAILSF_VALUE = 0x0;
        public const int MIXER_GETLINECONTROLSF_ONEBYTYPE = 0x2;
        public const int MIXER_SETCONTROLDETAILSF_VALUE = 0x0;
        public const int MIXERLINE_COMPONENTTYPE_DST_FIRST = 0x0;
        public const int MIXERLINE_COMPONENTTYPE_SRC_FIRST = 0x1000;        
        public const int MIXERCONTROL_CT_CLASS_FADER = 0x50000000;
        public const int MIXERCONTROL_CT_UNITS_UNSIGNED = 0x30000;
        public const int MIXERCONTROL_CONTROLTYPE_FADER = (MIXERCONTROL_CT_CLASS_FADER | MIXERCONTROL_CT_UNITS_UNSIGNED);
        public const int MIXERCONTROL_CONTROLTYPE_VOLUME = (MIXERCONTROL_CONTROLTYPE_FADER + 1);
        public const int MIXERLINE_COMPONENTTYPE_DST_SPEAKERS = (MIXERLINE_COMPONENTTYPE_DST_FIRST + 4);
        public const int MIXERLINE_COMPONENTTYPE_SRC_MICROPHONE = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 3);
        public const int MIXERLINE_COMPONENTTYPE_SRC_LINE = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 2);
#endif

        private const uint MIXERCONTROL_CT_CLASS_MASK = 0xF0000000u;
        private const uint MIXERCONTROL_CT_CLASS_CUSTOM = 0x00000000u;
        private const uint MIXERCONTROL_CT_CLASS_METER = 0x10000000u;
        private const uint MIXERCONTROL_CT_CLASS_SWITCH = 0x20000000u;
        private const uint MIXERCONTROL_CT_CLASS_NUMBER = 0x30000000u;
        private const uint MIXERCONTROL_CT_CLASS_SLIDER = 0x40000000u;
        private const uint MIXERCONTROL_CT_CLASS_FADER = 0x50000000u;
        private const uint MIXERCONTROL_CT_CLASS_TIME = 0x60000000u;
        private const uint MIXERCONTROL_CT_CLASS_LIST = 0x70000000u;

        private const uint MIXERCONTROL_CT_SUBCLASS_MASK = 0x0F000000u;

        private const uint MIXERCONTROL_CT_SC_SWITCH_BOOLEAN = 0x00000000u;
        private const uint MIXERCONTROL_CT_SC_SWITCH_BUTTON = 0x01000000u;

        private const uint MIXERCONTROL_CT_SC_METER_POLLED = 0x00000000u;

        private const uint MIXERCONTROL_CT_SC_TIME_MICROSECS = 0x00000000u;
        private const uint MIXERCONTROL_CT_SC_TIME_MILLISECS = 0x01000000u;

        private const uint MIXERCONTROL_CT_SC_LIST_SINGLE = 0x00000000u;
        private const uint MIXERCONTROL_CT_SC_LIST_MULTIPLE = 0x01000000u;

        private const uint MIXERCONTROL_CT_UNITS_MASK = 0x00FF0000u;
        private const uint MIXERCONTROL_CT_UNITS_CUSTOM = 0x00000000u;
        private const uint MIXERCONTROL_CT_UNITS_BOOLEAN = 0x00010000u;
        private const uint MIXERCONTROL_CT_UNITS_SIGNED = 0x00020000u;
        private const uint MIXERCONTROL_CT_UNITS_UNSIGNED = 0x00030000u;
        private const uint MIXERCONTROL_CT_UNITS_DECIBELS = 0x00040000u; /* in 10ths */
        private const uint MIXERCONTROL_CT_UNITS_PERCENT = 0x00050000u; /* in 10ths */


        /// <summary>Commonly used control types for specifying MIXERCONTROL.dwControlType</summary>
        public enum MIXERCONTROL_CONTROLTYPE : uint
        {
            CUSTOM = (MIXERCONTROL_CT_CLASS_CUSTOM | MIXERCONTROL_CT_UNITS_CUSTOM),
            BOOLEANMETER = (MIXERCONTROL_CT_CLASS_METER | MIXERCONTROL_CT_SC_METER_POLLED | MIXERCONTROL_CT_UNITS_BOOLEAN),
            SIGNEDMETER = (MIXERCONTROL_CT_CLASS_METER | MIXERCONTROL_CT_SC_METER_POLLED | MIXERCONTROL_CT_UNITS_SIGNED),
            PEAKMETER = (SIGNEDMETER + 1),
            UNSIGNEDMETER = (MIXERCONTROL_CT_CLASS_METER | MIXERCONTROL_CT_SC_METER_POLLED | MIXERCONTROL_CT_UNITS_UNSIGNED),
            BOOLEAN = (MIXERCONTROL_CT_CLASS_SWITCH | MIXERCONTROL_CT_SC_SWITCH_BOOLEAN | MIXERCONTROL_CT_UNITS_BOOLEAN),
            ONOFF = (BOOLEAN + 1),
            MUTE = (BOOLEAN + 2),
            MONO = (BOOLEAN + 3),
            LOUDNESS = (BOOLEAN + 4),
            STEREOENH = (BOOLEAN + 5),
            BASS_BOOST = (BOOLEAN + 0x00002277),
            BUTTON = (MIXERCONTROL_CT_CLASS_SWITCH | MIXERCONTROL_CT_SC_SWITCH_BUTTON | MIXERCONTROL_CT_UNITS_BOOLEAN),
            DECIBELS = (MIXERCONTROL_CT_CLASS_NUMBER | MIXERCONTROL_CT_UNITS_DECIBELS),
            SIGNED = (MIXERCONTROL_CT_CLASS_NUMBER | MIXERCONTROL_CT_UNITS_SIGNED),
            UNSIGNED = (MIXERCONTROL_CT_CLASS_NUMBER | MIXERCONTROL_CT_UNITS_UNSIGNED),
            PERCENT = (MIXERCONTROL_CT_CLASS_NUMBER | MIXERCONTROL_CT_UNITS_PERCENT),
            SLIDER = (MIXERCONTROL_CT_CLASS_SLIDER | MIXERCONTROL_CT_UNITS_SIGNED),
            PAN = (SLIDER + 1),
            QSOUNDPAN = (SLIDER + 2),
            FADER = (MIXERCONTROL_CT_CLASS_FADER | MIXERCONTROL_CT_UNITS_UNSIGNED),
            VOLUME = (FADER + 1),
            BASS = (FADER + 2),
            TREBLE = (FADER + 3),
            EQUALIZER = (FADER + 4),
            SINGLESELECT = (MIXERCONTROL_CT_CLASS_LIST | MIXERCONTROL_CT_SC_LIST_SINGLE | MIXERCONTROL_CT_UNITS_BOOLEAN),
            MUX = (SINGLESELECT + 1),
            MULTIPLESELECT = (MIXERCONTROL_CT_CLASS_LIST | MIXERCONTROL_CT_SC_LIST_MULTIPLE | MIXERCONTROL_CT_UNITS_BOOLEAN),
            MIXER = (MULTIPLESELECT + 1),
            MICROTIME = (MIXERCONTROL_CT_CLASS_TIME | MIXERCONTROL_CT_SC_TIME_MICROSECS | MIXERCONTROL_CT_UNITS_UNSIGNED),
            MILLITIME = (MIXERCONTROL_CT_CLASS_TIME | MIXERCONTROL_CT_SC_TIME_MILLISECS | MIXERCONTROL_CT_UNITS_UNSIGNED),
        }
        public enum MIXERLINE_TARGETTYPE : uint
        {
            UNDEFINED = 0,
            WAVEOUT = 1,
            WAVEIN = 2,
            MIDIOUT = 3,
            MIDIIN = 4,
            AUX = 5,
        }
        public enum MIXERLINE_LINEF : uint
        {
            ACTIVE = 0x00000001u,
            DISCONNECTED = 0x00008000u,
            SOURCE = 0x80000000u,
        }
        public enum MIXERLINE_COMPONENTTYPE : uint
        {
            DST_FIRST = 0x00000000u,
            DST_UNDEFINED = (DST_FIRST + 0),
            DST_DIGITAL = (DST_FIRST + 1),
            DST_LINE = (DST_FIRST + 2),
            DST_MONITOR = (DST_FIRST + 3),
            DST_SPEAKERS = (DST_FIRST + 4),
            DST_HEADPHONES = (DST_FIRST + 5),
            DST_TELEPHONE = (DST_FIRST + 6),
            DST_WAVEIN = (DST_FIRST + 7),
            DST_VOICEIN = (DST_FIRST + 8),
            DST_LAST = (DST_FIRST + 8),
            SRC_FIRST = 0x00001000u,
            SRC_UNDEFINED = (SRC_FIRST + 0),
            SRC_DIGITAL = (SRC_FIRST + 1),
            SRC_LINE = (SRC_FIRST + 2),
            SRC_MICROPHONE = (SRC_FIRST + 3),
            SRC_SYNTHESIZER = (SRC_FIRST + 4),
            SRC_COMPACTDISC = (SRC_FIRST + 5),
            SRC_TELEPHONE = (SRC_FIRST + 6),
            SRC_PCSPEAKER = (SRC_FIRST + 7),
            SRC_WAVEOUT = (SRC_FIRST + 8),
            SRC_AUXILIARY = (SRC_FIRST + 9),
            SRC_ANALOG = (SRC_FIRST + 10),
            SRC_LAST = (SRC_FIRST + 10),
        }

        public enum MIXER_OBJECTF : uint
        {
            HANDLE = 0x80000000u,
            MIXER = 0x00000000u,
            HMIXER = (HANDLE | MIXER),
            WAVEOUT = 0x10000000u,
            HWAVEOUT = (HANDLE | WAVEOUT),
            WAVEIN = 0x20000000u,
            HWAVEIN = (HANDLE | WAVEIN),
            MIDIOUT = 0x30000000u,
            HMIDIOUT = (HANDLE | MIDIOUT),
            MIDIIN = 0x40000000u,
            HMIDIIN = (HANDLE | MIDIIN),
            AUX = 0x50000000u,
        }

        public enum MIXER_GETLINEINFOF : uint
        {
            DESTINATION = 0x00000000u,
            SOURCE = 0x00000001u,
            LINEID = 0x00000002u,
            COMPONENTTYPE = 0x00000003u,
            TARGETTYPE = 0x00000004u,
            QUERYMASK = 0x0000000Fu,
        }

    }

    public enum MIXER_FLAGS
    {
        MIXER_OBJECTF_HANDLE = unchecked((int)0x80000000),
        MIXER_OBJECTF_MIXER = 0,
        MIXER_OBJECTF_HMIXER = MIXER_OBJECTF_MIXER | MIXER_OBJECTF_HANDLE,
        MIXER_OBJECTF_WAVEOUT = 0x10000000,
        MIXER_OBJECTF_HWAVEOUT = MIXER_OBJECTF_WAVEOUT | MIXER_OBJECTF_HANDLE,
        MIXER_OBJECTF_WAVEIN = 0x20000000,
        MIXER_OBJECTF_HWAVEIN = MIXER_OBJECTF_WAVEIN | MIXER_OBJECTF_HANDLE,
        MIXER_OBJECTF_MIDIOUT = 0x30000000,
        MIXER_OBJECTF_HMIDIOUT = MIXER_OBJECTF_MIDIOUT | MIXER_OBJECTF_HANDLE,
        MIXER_OBJECTF_MIDIIN = 0x40000000,
        MIXER_OBJECTF_HMIDIIN = MIXER_OBJECTF_MIDIIN | MIXER_OBJECTF_HANDLE,
        MIXER_OBJECTF_AUX = 0x50000000,

        MIXER_GETCONTROLDETAILSF_VALUE = 0,
        MIXER_SETCONTROLDETAILSF_VALUE = 0,
        MIXER_GETCONTROLDETAILSF_LISTTEXT = 1,
        MIXER_SETCONTROLDETAILSF_LISTTEXT = 1,
        MIXER_GETCONTROLDETAILSF_QUERYMASK = 0xF,
        MIXER_SETCONTROLDETAILSF_QUERYMASK = 0xF,
        MIXER_GETLINECONTROLSF_QUERYMASK = 0xF,
    }

    public enum MixerLineInfoType
    {
        MIXER_GETLINECONTROLSF_ALL = 0,
        MIXER_GETLINECONTROLSF_ONEBYID = 1,
        MIXER_GETLINECONTROLSF_ONEBYTYPE = 2,

        MIXER_GETLINEINFOF_DESTINATION = 0,
        MIXER_GETLINEINFOF_SOURCE = 1,
        MIXER_GETLINEINFOF_LINEID = 2,
        MIXER_GETLINEINFOF_COMPONENTTYPE = 3,
        MIXER_GETLINEINFOF_TARGETTYPE = 4,
        MIXER_GETLINEINFOF_QUERYMASK = 0xF,
    }

    public enum MixerLineComponentType
    {
        MIXERLINE_COMPONENTTYPE_DST_UNDEFINED = 0x0,
        MIXERLINE_COMPONENTTYPE_DST_DIGITAL = 0x1,
        MIXERLINE_COMPONENTTYPE_DST_LINE = 0x2,
        MIXERLINE_COMPONENTTYPE_DST_MONITOR = 0x3,
        MIXERLINE_COMPONENTTYPE_DST_SPEAKERS = 0x4,
        MIXERLINE_COMPONENTTYPE_DST_HEADPHONES = 0x5,
        MIXERLINE_COMPONENTTYPE_DST_TELEPHONE = 0x6,
        MIXERLINE_COMPONENTTYPE_DST_WAVEIN = 0x7,
        MIXERLINE_COMPONENTTYPE_DST_VOICEIN = 0x8,
        MIXERLINE_COMPONENTTYPE_SRC_UNDEFINED = 0x1000,
        MIXERLINE_COMPONENTTYPE_SRC_DIGITAL = 0x1001,
        MIXERLINE_COMPONENTTYPE_SRC_LINE = 0x1002,
        MIXERLINE_COMPONENTTYPE_SRC_MICROPHONE = 0x1003,
        MIXERLINE_COMPONENTTYPE_SRC_SYNTHESIZER = 0x1004,
        MIXERLINE_COMPONENTTYPE_SRC_COMPACTDISC = 0x1005,
        MIXERLINE_COMPONENTTYPE_SRC_TELEPHONE = 0x1006,
        MIXERLINE_COMPONENTTYPE_SRC_PCSPEAKER = 0x1007,
        MIXERLINE_COMPONENTTYPE_SRC_WAVEOUT = 0x1008,
        MIXERLINE_COMPONENTTYPE_SRC_AUXILIARY = 0x1009,
        MIXERLINE_COMPONENTTYPE_SRC_ANALOG = 0x100A,
    }

    public enum AudioLineType
    {
        MIXERLINE_TARGETTYPE_UNDEFINED = 0,
        MIXERLINE_TARGETTYPE_WAVEOUT = 1,
        MIXERLINE_TARGETTYPE_WAVEIN = 2,
        MIXERLINE_TARGETTYPE_MIDIOUT = 3,
        MIXERLINE_TARGETTYPE_MIDIIN = 4,
        MIXERLINE_TARGETTYPE_AUX = 5
    }

    public enum MMRESULT : uint
    {
        MMSYSERR_NOERROR = 0,
        MMSYSERR_ERROR = 1,
        MMSYSERR_BADDEVICEID = 2,
        MMSYSERR_NOTENABLED = 3,
        MMSYSERR_ALLOCATED = 4,
        MMSYSERR_INVALHANDLE = 5,
        MMSYSERR_NODRIVER = 6,
        MMSYSERR_NOMEM = 7,
        MMSYSERR_NOTSUPPORTED = 8,
        MMSYSERR_BADERRNUM = 9,
        MMSYSERR_INVALFLAG = 10,
        MMSYSERR_INVALPARAM = 11,
        MMSYSERR_HANDLEBUSY = 12,
        MMSYSERR_INVALIDALIAS = 13,
        MMSYSERR_BADDB = 14,
        MMSYSERR_KEYNOTFOUND = 15,
        MMSYSERR_READERROR = 16,
        MMSYSERR_WRITEERROR = 17,
        MMSYSERR_DELETEERROR = 18,
        MMSYSERR_VALNOTFOUND = 19,
        MMSYSERR_NODRIVERCB = 20,
        WAVERR_BADFORMAT = 32,
        WAVERR_STILLPLAYING = 33,
        WAVERR_UNPREPARED = 34
    }
}
