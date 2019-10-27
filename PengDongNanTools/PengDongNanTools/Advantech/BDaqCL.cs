using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;

//研华DAQ系列I/O采集卡C#库函数
namespace Automation.BDaq
{
#region Bionic DAQ types

   public enum TerminalBoard
   {
      WiringBoard = 0,
      PCLD8710,
      PCLD789,
      PCLD8115,
   }

   public enum ModuleType
   {
      DaqAny   = -1,
      DaqGroup = 1,
      DaqDevice,
      DaqAi,
      DaqAo,
      DaqDio,
      DaqCounter,
   }

   public enum AccessMode
   {
      ModeRead = 0,
      ModeWrite,
      ModeWriteWithReset,
   }

   [Flags]
   public enum MathIntervalType
   {
      /* Right boundary definition, define the maximum value state, use the bit 0,1 */
      RightOpenSet        = 0x0, /* No maximum value limitation.  */
      RightClosedBoundary = 0x1,	/* The maximum value is included. */
      RightOpenBoundary   = 0x2, /* The maximum value is not included. */

      /* Left boundary definition, define the minimum value state, used the bit 2, 3 */
      LeftOpenSet        = 0x0,	/* No minimum value limitation. */
      LeftClosedBoundary = 0x4, 	/* The minimum value is included. */
      LeftOpenBoundary   = 0x8,	/* The minimum value is not included */

      /* The signality expression */
      Boundless = 0x0,  /* Boundless set. (LeftOpenSet | RightOpenSet) */

      /* The combination notation */
      LOSROS = 0x0,	 /* ( LeftOpenSet | RightOpenSet), algebra notation: ( un-limit, max) */
      LOSRCB = 0x1,	 /* ( LeftOpenSet | RightClosedBoundary), algebra notation: ( un-limit, max ] */
      LOSROB = 0x2,	 /* ( LeftOpenSet | RightOpenBoundary), algebra notation: ( un-limit, max) */

      LCBROS = 0x4,	 /* ( LeftClosedBoundary | RightOpenSet), algebra notation: [min, un-limit) */
      LCBRCB = 0x5,	 /* ( LeftClosedBoundary | RightClosedBoundary), algebra notation: [ min, right ] */
      LCBROB = 0x6,	 /* ( LeftClosedBoundary | RightOpenBoundary), algebra notation: [ min, right) */

      LOBROS = 0x8,	 /* ( LeftOpenBoundary | RightOpenSet), algebra notation: ( min, un-limit) */
      LOBRCB = 0x9,	 /* ( LeftOpenBoundary | RightClosedBoundary), algebra notation: ( min, right ] */
      LOBROB = 0xA,	 /* ( LeftOpenBoundary | RightOpenBoundary), algebra notation: ( min, right) */
   }

   [StructLayout(LayoutKind.Sequential)]
   public struct MathInterval
   {
      public int    Type;
      public double Min;
      public double Max;
   }

   public enum AiChannelType
   {
      AllSingleEnded = 0,
      AllDifferential,
      AllSeDiffAdj,
      MixedSeDiffAdj,
   }

   public enum AiSignalType
   {
      SingleEnded = 0,
      Differential,
   }

   public enum DioPortType
   {
      PortDi = 0,        // the port number references to a DI port
      PortDo,            // the port number references to a DO port
      PortDio,           // the port number references to a DI port and a DO port
      Port8255A,         // the port number references to a PPI port A mode DIO port.
      Port8255C,         // the port number references to a PPI port C mode DIO port.
      PortIndvdlDio,     // the port number references to a port whose each channel can be configured as in or out.
   }

   [Flags]
   public enum DioPortDir
   {
      Input   = 0x00,
      LoutHin = 0x0F,
      LinHout = 0xF0,
      Output  = 0xFF,
   }

   public enum SamplingMethod
   {
      EqualTimeSwitch = 0,
      Simultaneous,
   }

   public enum TemperatureDegree
   {
      Celsius = 0,
      Fahrenheit,
      Rankine,
      Kelvin,
   }

   public enum BurnoutRetType
   {
      Current = 0,
      ParticularValue,
      UpLimit,
      LowLimit,
      LastCorrectValue,
   }

   public enum ValueUnit
   {
      Kilovolt,      /* KV */
      Volt,          /* V  */
      Millivolt,     /* mV */
      Microvolt,     /* uV */
      Kiloampere,    /* KA */
      Ampere,        /* A  */
      Milliampere,   /* mA */
      Microampere,   /* uA */
      CelsiusUnit,   /* Celsius */
   }

   public enum ValueRange
   {
      V_OMIT = -1,            /* Unknown when get, ignored when set */
      V_Neg15To15 = 0,        /* +/- 15 V  */
      V_Neg10To10,            /* +/- 10 V  */
      V_Neg5To5,              /* +/- 5 V */
      V_Neg2pt5To2pt5,        /* +/- 2.5 V */
      V_Neg1pt25To1pt25,      /* +/- 1.25 V */
      V_Neg1To1,              /* +/- 1 V */

      V_0To15,                /* 0~15 V  */
      V_0To10,                /* 0~10 V  */
      V_0To5,                 /* 0~5 V */
      V_0To2pt5,              /* 0~2.5 V */
      V_0To1pt25,             /* 0~1.25 V */
      V_0To1,                 /* 0~1 V */

      mV_Neg625To625,         /* +/- 625mV */
      mV_Neg500To500,         /* +/- 500 mV */
      mV_Neg312pt5To312pt5,   /* +/- 312.5 mV */
      mV_Neg200To200,         /* +/- 200 mV */
      mV_Neg150To150,         /* +/- 150 mV */
      mV_Neg100To100,         /* +/- 100 mV */
      mV_Neg50To50,           /* +/- 50 mV */
      mV_Neg30To30,           /* +/- 30 mV */
      mV_Neg20To20,           /* +/- 20 mV */
      mV_Neg15To15,           /* +/- 15 mV  */
      mV_Neg10To10,           /* +/- 10 mV */
      mV_Neg5To5,             /* +/- 5 mV */

      mV_0To625,              /* 0 ~ 625 mV */
      mV_0To500,              /* 0 ~ 500 mV */
      mV_0To150,              /* 0 ~ 150 mV */
      mV_0To100,              /* 0 ~ 100 mV */
      mV_0To50,               /* 0 ~ 50 mV */
      mV_0To20,               /* 0 ~ 20 mV */
      mV_0To15,               /* 0 ~ 15 mV */
      mV_0To10,               /* 0 ~ 10 mV */

      mA_Neg20To20,           /* +/- 20mA */
      mA_0To20,               /* 0 ~ 20 mA */
      mA_4To20,               /* 4 ~ 20 mA */
      mA_0To24,               /* 0 ~ 24 mA */

      /* For USB4702_4704 */
      V_Neg2To2,              /* +/- 2 V */
      V_Neg4To4,              /* +/- 4 V */
      V_Neg20To20,            /* +/- 20 V */

      Jtype_0To760C = 0x8000, /* T/C J type 0~760 'C */
      Ktype_0To1370C,		   /* T/C K type 0~1370 'C */
      Ttype_Neg100To400C,     /* T/C T type -100~400 'C */
      Etype_0To1000C,		   /* T/C E type 0~1000 'C */
      Rtype_500To1750C,	   /* T/C R type 500~1750 'C */
      Stype_500To1750C,	   /* T/C S type 500~1750 'C */
      Btype_500To1800C,	   /* T/C B type 500~1800 'C */

      Pt392_Neg50To150,	   /* Pt392 -50~150 'C  */
      Pt385_Neg200To200,	   /* Pt385 -200~200 'C */
      Pt385_0To400,		   /* Pt385 0~400 'C */
      Pt385_Neg50To150,	   /* Pt385 -50~150 'C */
      Pt385_Neg100To100,      /* Pt385 -100~100 'C */
      Pt385_0To100,		   /* Pt385 0~100 'C  */
      Pt385_0To200,		   /* Pt385 0~200 'C */
      Pt385_0To600,		   /* Pt385 0~600 'C */
      Pt392_Neg100To100,      /* Pt392 -100~100 'C  */
      Pt392_0To100,           /* Pt392 0~100 'C */
      Pt392_0To200,           /* Pt392 0~200 'C */
      Pt392_0To600,           /* Pt392 0~600 'C */
      Pt392_0To400,           /* Pt392 0~400 'C */
      Pt392_Neg200To200,      /* Pt392 -200~200 'C  */
      Pt1000_Neg40To160,      /* Pt1000 -40~160 'C  */

      Balcon500_Neg30To120,   /* Balcon500 -30~120 'C  */

      Ni518_Neg80To100,       /* Ni518 -80~100 'C */
      Ni518_0To100,           /* Ni518 0~100 'C */
      Ni508_0To100,           /* Ni508 0~100 'C */
      Ni508_Neg50To200,       /* Ni508 -50~200 'C */

      Thermistor_3K_0To100,   /* Thermistor 3K 0~100 'C */
      Thermistor_10K_0To100,  /* Thermistor 10K 0~100 'C */

      Jtype_Neg210To1200C,    /* T/C J type -210~1200 'C */
      Ktype_Neg270To1372C,    /* T/C K type -270~1372 'C */
      Ttype_Neg270To400C,     /* T/C T type -270~400 'C */
      Etype_Neg270To1000C,		/* T/C E type -270~1000 'C */
      Rtype_Neg50To1768C,	   /* T/C R type -50~1768 'C */
      Stype_Neg50To1768C,	   /* T/C S type -50~1768 'C */
      Btype_40To1820C,	      /* T/C B type 40~1820 'C */

      Jtype_Neg210To870C,     /* T/C J type -210~870 'C */
      Rtype_0To1768C,	      /* T/C R type 0~1768 'C */
      Stype_0To1768C,	      /* T/C S type 0~1768 'C */

      /* 0xC000 ~ 0xF000 : user customized value range type */
      UserCustomizedVrgStart = 0xC000,
      UserCustomizedVrgEnd = 0xF000,

      /* AO external reference type */
      V_ExternalRefBipolar = 0xF001, /* External reference voltage unipolar */
      V_ExternalRefUnipolar = 0xF002, /* External reference voltage bipolar */
   }

   public enum SignalPolarity
   {
      Negative = 0,
      Positive,
   }

   public enum SignalCountingType
   {
      DownCount = 0,  /* counter value decreases on each clock */
      UpCount,        /* counter value increases on each clock */
      PulseDirection, /* counting direction is determined by two signals, one is clock, the other is direction signal */
      TwoPulse,       /* counting direction is determined by two signals, one is up-counting signal, the other is down-counting signal */
      AbPhaseX1,      /* AB phase, 1x rate up/down counting */
      AbPhaseX2,      /* AB phase, 2x rate up/down counting */
      AbPhaseX4,      /* AB phase, 4x rate up/down counting */
   }

   public enum OutSignalType
   {
      OutSignalNone = 0,  /* no output or output is 'disabled' */
      ChipDefined,        /* hardware chip defined */
      NegChipDefined,     /* hardware chip defined, negative logical */
      PositivePulse,      /* a low-to-high pulse */
      NegativePulse,      /* a high-to-low pulse */
      ToggledFromLow,     /* the level toggled from low to high */
      ToggledFromHigh,    /* the level toggled from high to low */
   }

   public enum CounterCapability
   {
      Primary = 1,
      InstantEventCount,
      OneShot,
      TimerPulse,
      InstantFreqMeter,
      InstantPwmIn,
      InstantPwmOut,
      SnapCount,
   }

   public enum CounterOperationMode
   {
      C8254_M0 = 0, /*8254 mode 0, interrupt on terminal count */
      C8254_M1,     /*8254 mode 1, hardware retriggerable one-shot */
      C8254_M2,     /*8254 mode 2, rate generator */
      C8254_M3,     /*8254 mode 3, square save mode */
      C8254_M4,     /*8254 mode 4, software triggered strobe */
      C8254_M5,     /*8254 mode 5, hardware triggered strobe */

      C1780_MA,	/* Mode A level & pulse out, Software-Triggered without Hardware Gating */
      C1780_MB,	/* Mode B level & pulse out, Software-Triggered with Level Gating, = 8254_M0 */
      C1780_MC,	/* Mode C level & pulse out, Hardware-triggered strobe level */
      C1780_MD,	/* Mode D level & Pulse out, Rate generate with no hardware gating */
      C1780_ME,	/* Mode E level & pulse out, Rate generator with level Gating */
      C1780_MF,	/* Mode F level & pulse out, Non-retriggerable One-shot (Pulse type = 8254_M1) */
      C1780_MG,	/* Mode G level & pulse out, Software-triggered delayed pulse one-shot */
      C1780_MH,	/* Mode H level & pulse out, Software-triggered delayed pulse one-shot with hardware gating */
      C1780_MI,	/* Mode I level & pulse out, Hardware-triggered delay pulse strobe */
      C1780_MJ,	/* Mode J level & pulse out, Variable Duty Cycle Rate Generator with No Hardware Gating */
      C1780_MK,	/* Mode K level & pulse out, Variable Duty Cycle Rate Generator with Level Gating */
      C1780_ML,	/* Mode L level & pulse out, Hardware-Triggered Delayed Pulse One-Shot */
      C1780_MO,	/* Mode O level & pulse out, Hardware-Triggered Strobe with Edge Disarm */
      C1780_MR,	/* Mode R level & pulse out, Non-Retriggerbale One-Shot with Edge Disarm */
      C1780_MU,	/* Mode U level & pulse out, Hardware-Triggered Delayed Pulse Strobe with Edge Disarm */
      C1780_MX,	/* Mode X level & pulse out, Hardware-Triggered Delayed Pulse One-Shot with Edge Disarm */
   }

   public enum CounterValueRegister
   {
      CntLoad,
      CntPreset = CntLoad,
      CntHold,
      CntOverCompare,
      CntUnderCompare,
   }

   public enum CounterCascadeGroup
   {
      GroupNone = 0,    /* no cascade*/
      Cnt0Cnt1,	      /* Counter 0 as first, counter 1 as second. */
      Cnt2Cnt3,	      /* Counter 2 as first, counter 3 as second */
      Cnt4Cnt5,	      /* Counter 4 as first, counter 5 as second */
      Cnt6Cnt7,	      /* Counter 6 as first, counter 7 as second */
   }

   public enum FreqMeasureMethod
   {
      AutoAdaptive = 0, 		   /* Intelligently select the measurement method according to the input signal. */
      CountingPulseBySysTime, 	/* Using system timing clock to calculate the frequency */
      CountingPulseByDevTime,	   /* Using the device timing clock to calculate the frequency */
      PeriodInverse, 		      /* Calculate the frequency from the period of the signal */
   }

   public enum ActiveSignal
   {
      ActiveNone = 0,
      RisingEdge,
      FallingEdge,
      BothEdge,
      HighLevel,
      LowLevel,
   }

   public enum TriggerAction
   {
      ActionNone = 0,   /* No action to take even if the trigger condition is satisfied */
      DelayToStart,     /* Begin to start after the specified time is elapsed if the trigger condition is satisfied */
      DelayToStop,      /* Stop execution after the specified time is elapsed if the trigger condition is satisfied */
   }

   public enum SignalPosition
   {
      InternalSig = 0,
      OnConnector,
      OnAmsi,
   }

   public enum SignalDrop
   {
      SignalNone = 0,      /* No connection */

      /*Internal signal connector*/
      SigInternalClock,        /* Device built-in clock, If the device has several built-in clock, this represent the highest freq one. */
      SigInternal1KHz,         /* Device built-in clock, 1KHz */
      SigInternal10KHz,        /* Device built-in clock, 10KHz */
      SigInternal100KHz,       /* Device built-in clock, 100KHz */
      SigInternal1MHz,         /* Device built-in clock, 1MHz */
      SigInternal10MHz,        /* Device built-in clock, 10MHz */
      SigInternal20MHz,        /* Device built-in clock, 20MHz */
      SigInternal30MHz,        /* Device built-in clock, 30MHz */
      SigInternal40MHz,        /* Device built-in clock, 40MHz */
      SigInternal50MHz,        /* Device built-in clock, 50MHz */
      SigInternal60MHz,        /* Device built-in clock, 60MHz */

      SigDiPatternMatch,       /* When DI pattern match occurred */
      SigDiStatusChange,       /* When DI status change occurred */

      /*Function pin on connector*/
      SigExtAnaClock,          /* Analog clock pin of connector */
      SigExtAnaScanClock,      /* scan clock pin of connector */
      SigExtAnaTrigger,        /* external analog trigger pin of connector */
      SigExtDigClock,          /* digital clock pin of connector */
      SigExtDigTrigger0,       /* external digital trigger 0 pin( or DI start trigger pin) of connector */
      SigExtDigTrigger1,       /* external digital trigger 1 pin( or DI stop trigger pin) of connector  */
      SigExtDigTrigger2,       /* external digital trigger 2 pin( or DO start trigger pin) of connector */
      SigExtDigTrigger3,       /* external digital trigger 3 pin( or DO stop trigger pin) of connector  */
      SigCHFrzDo,              /* Channel freeze DO ports pin */

      /*Signal source or target on the connector*/
      /*AI channel pins*/
      SigAi0, SigAi1, SigAi2, SigAi3, SigAi4, SigAi5, SigAi6, SigAi7,
      SigAi8, SigAi9, SigAi10, SigAi11, SigAi12, SigAi13, SigAi14, SigAi15,
      SigAi16, SigAi17, SigAi18, SigAi19, SigAi20, SigAi21, SigAi22, SigAi23,
      SigAi24, SigAi25, SigAi26, SigAi27, SigAi28, SigAi29, SigAi30, SigAi31,
      SigAi32, SigAi33, SigAi34, SigAi35, SigAi36, SigAi37, SigAi38, SigAi39,
      SigAi40, SigAi41, SigAi42, SigAi43, SigAi44, SigAi45, SigAi46, SigAi47,
      SigAi48, SigAi49, SigAi50, SigAi51, SigAi52, SigAi53, SigAi54, SigAi55,
      SigAi56, SigAi57, SigAi58, SigAi59, SigAi60, SigAi61, SigAi62, SigAi63,

      /*AO channel pins*/
      SigAo0, SigAo1, SigAo2, SigAo3, SigAo4, SigAo5, SigAo6, SigAo7,
      SigAo8, SigAo9, SigAo10, SigAo11, SigAo12, SigAo13, SigAo14, SigAo15,
      SigAo16, SigAo17, SigAo18, SigAo19, SigAo20, SigAo21, SigAo22, SigAo23,
      SigAo24, SigAo25, SigAo26, SigAo27, SigAo28, SigAo29, SigAo30, SigAo31,

      /*DI pins*/
      SigDi0, SigDi1, SigDi2, SigDi3, SigDi4, SigDi5, SigDi6, SigDi7,
      SigDi8, SigDi9, SigDi10, SigDi11, SigDi12, SigDi13, SigDi14, SigDi15,
      SigDi16, SigDi17, SigDi18, SigDi19, SigDi20, SigDi21, SigDi22, SigDi23,
      SigDi24, SigDi25, SigDi26, SigDi27, SigDi28, SigDi29, SigDi30, SigDi31,
      SigDi32, SigDi33, SigDi34, SigDi35, SigDi36, SigDi37, SigDi38, SigDi39,
      SigDi40, SigDi41, SigDi42, SigDi43, SigDi44, SigDi45, SigDi46, SigDi47,
      SigDi48, SigDi49, SigDi50, SigDi51, SigDi52, SigDi53, SigDi54, SigDi55,
      SigDi56, SigDi57, SigDi58, SigDi59, SigDi60, SigDi61, SigDi62, SigDi63,
      SigDi64, SigDi65, SigDi66, SigDi67, SigDi68, SigDi69, SigDi70, SigDi71,
      SigDi72, SigDi73, SigDi74, SigDi75, SigDi76, SigDi77, SigDi78, SigDi79,
      SigDi80, SigDi81, SigDi82, SigDi83, SigDi84, SigDi85, SigDi86, SigDi87,
      SigDi88, SigDi89, SigDi90, SigDi91, SigDi92, SigDi93, SigDi94, SigDi95,
      SigDi96, SigDi97, SigDi98, SigDi99, SigDi100, SigDi101, SigDi102, SigDi103,
      SigDi104, SigDi105, SigDi106, SigDi107, SigDi108, SigDi109, SigDi110, SigDi111,
      SigDi112, SigDi113, SigDi114, SigDi115, SigDi116, SigDi117, SigDi118, SigDi119,
      SigDi120, SigDi121, SigDi122, SigDi123, SigDi124, SigDi125, SigDi126, SigDi127,
      SigDi128, SigDi129, SigDi130, SigDi131, SigDi132, SigDi133, SigDi134, SigDi135,
      SigDi136, SigDi137, SigDi138, SigDi139, SigDi140, SigDi141, SigDi142, SigDi143,
      SigDi144, SigDi145, SigDi146, SigDi147, SigDi148, SigDi149, SigDi150, SigDi151,
      SigDi152, SigDi153, SigDi154, SigDi155, SigDi156, SigDi157, SigDi158, SigDi159,
      SigDi160, SigDi161, SigDi162, SigDi163, SigDi164, SigDi165, SigDi166, SigDi167,
      SigDi168, SigDi169, SigDi170, SigDi171, SigDi172, SigDi173, SigDi174, SigDi175,
      SigDi176, SigDi177, SigDi178, SigDi179, SigDi180, SigDi181, SigDi182, SigDi183,
      SigDi184, SigDi185, SigDi186, SigDi187, SigDi188, SigDi189, SigDi190, SigDi191,
      SigDi192, SigDi193, SigDi194, SigDi195, SigDi196, SigDi197, SigDi198, SigDi199,
      SigDi200, SigDi201, SigDi202, SigDi203, SigDi204, SigDi205, SigDi206, SigDi207,
      SigDi208, SigDi209, SigDi210, SigDi211, SigDi212, SigDi213, SigDi214, SigDi215,
      SigDi216, SigDi217, SigDi218, SigDi219, SigDi220, SigDi221, SigDi222, SigDi223,
      SigDi224, SigDi225, SigDi226, SigDi227, SigDi228, SigDi229, SigDi230, SigDi231,
      SigDi232, SigDi233, SigDi234, SigDi235, SigDi236, SigDi237, SigDi238, SigDi239,
      SigDi240, SigDi241, SigDi242, SigDi243, SigDi244, SigDi245, SigDi246, SigDi247,
      SigDi248, SigDi249, SigDi250, SigDi251, SigDi252, SigDi253, SigDi254, SigDi255,

      /*DIO pins*/
      SigDio0,  SigDio1,  SigDio2,  SigDio3,  SigDio4,  SigDio5,  SigDio6,  SigDio7,
      SigDio8,  SigDio9,  SigDio10, SigDio11, SigDio12, SigDio13, SigDio14, SigDio15,
      SigDio16, SigDio17, SigDio18, SigDio19, SigDio20, SigDio21, SigDio22, SigDio23,
      SigDio24, SigDio25, SigDio26, SigDio27, SigDio28, SigDio29, SigDio30, SigDio31,
      SigDio32, SigDio33, SigDio34, SigDio35, SigDio36, SigDio37, SigDio38, SigDio39,
      SigDio40, SigDio41, SigDio42, SigDio43, SigDio44, SigDio45, SigDio46, SigDio47,
      SigDio48, SigDio49, SigDio50, SigDio51, SigDio52, SigDio53, SigDio54, SigDio55,
      SigDio56, SigDio57, SigDio58, SigDio59, SigDio60, SigDio61, SigDio62, SigDio63,
      SigDio64, SigDio65, SigDio66, SigDio67, SigDio68, SigDio69, SigDio70, SigDio71,
      SigDio72, SigDio73, SigDio74, SigDio75, SigDio76, SigDio77, SigDio78, SigDio79,
      SigDio80, SigDio81, SigDio82, SigDio83, SigDio84, SigDio85, SigDio86, SigDio87,
      SigDio88, SigDio89, SigDio90, SigDio91, SigDio92, SigDio93, SigDio94, SigDio95,
      SigDio96, SigDio97, SigDio98, SigDio99, SigDio100, SigDio101, SigDio102, SigDio103,
      SigDio104, SigDio105, SigDio106, SigDio107, SigDio108, SigDio109, SigDio110, SigDio111,
      SigDio112, SigDio113, SigDio114, SigDio115, SigDio116, SigDio117, SigDio118, SigDio119,
      SigDio120, SigDio121, SigDio122, SigDio123, SigDio124, SigDio125, SigDio126, SigDio127,
      SigDio128, SigDio129, SigDio130, SigDio131, SigDio132, SigDio133, SigDio134, SigDio135,
      SigDio136, SigDio137, SigDio138, SigDio139, SigDio140, SigDio141, SigDio142, SigDio143,
      SigDio144, SigDio145, SigDio146, SigDio147, SigDio148, SigDio149, SigDio150, SigDio151,
      SigDio152, SigDio153, SigDio154, SigDio155, SigDio156, SigDio157, SigDio158, SigDio159,
      SigDio160, SigDio161, SigDio162, SigDio163, SigDio164, SigDio165, SigDio166, SigDio167,
      SigDio168, SigDio169, SigDio170, SigDio171, SigDio172, SigDio173, SigDio174, SigDio175,
      SigDio176, SigDio177, SigDio178, SigDio179, SigDio180, SigDio181, SigDio182, SigDio183,
      SigDio184, SigDio185, SigDio186, SigDio187, SigDio188, SigDio189, SigDio190, SigDio191,
      SigDio192, SigDio193, SigDio194, SigDio195, SigDio196, SigDio197, SigDio198, SigDio199,
      SigDio200, SigDio201, SigDio202, SigDio203, SigDio204, SigDio205, SigDio206, SigDio207,
      SigDio208, SigDio209, SigDio210, SigDio211, SigDio212, SigDio213, SigDio214, SigDio215,
      SigDio216, SigDio217, SigDio218, SigDio219, SigDio220, SigDio221, SigDio222, SigDio223,
      SigDio224, SigDio225, SigDio226, SigDio227, SigDio228, SigDio229, SigDio230, SigDio231,
      SigDio232, SigDio233, SigDio234, SigDio235, SigDio236, SigDio237, SigDio238, SigDio239,
      SigDio240, SigDio241, SigDio242, SigDio243, SigDio244, SigDio245, SigDio246, SigDio247,
      SigDio248, SigDio249, SigDio250, SigDio251, SigDio252, SigDio253, SigDio254, SigDio255,

      /*Counter clock pins*/
      SigCntClk0, SigCntClk1, SigCntClk2, SigCntClk3, SigCntClk4, SigCntClk5, SigCntClk6, SigCntClk7,

      /*counter gate pins*/
      SigCntGate0, SigCntGate1, SigCntGate2, SigCntGate3, SigCntGate4, SigCntGate5, SigCntGate6, SigCntGate7,

      /*counter out pins*/
      SigCntOut0, SigCntOut1, SigCntOut2, SigCntOut3, SigCntOut4, SigCntOut5, SigCntOut6, SigCntOut7,

      /*counter frequency out pins*/
      SigCntFout0, SigCntFout1, SigCntFout2, SigCntFout3, SigCntFout4, SigCntFout5, SigCntFout6, SigCntFout7,

      /*AMSI pins*/
      SigAmsiPin0, SigAmsiPin1, SigAmsiPin2, SigAmsiPin3, SigAmsiPin4, SigAmsiPin5, SigAmsiPin6, SigAmsiPin7,
      SigAmsiPin8, SigAmsiPin9, SigAmsiPin10, SigAmsiPin11, SigAmsiPin12, SigAmsiPin13, SigAmsiPin14, SigAmsiPin15,
      SigAmsiPin16, SigAmsiPin17, SigAmsiPin18, SigAmsiPin19,

      /*new clocks*/
      SigInternal2Hz,         /* Device built-in clock, 2Hz */
      SigInternal20Hz,        /* Device built-in clock, 20Hz */
      SigInternal200Hz,       /* Device built-in clock, 200KHz */
      SigInternal2KHz,        /* Device built-in clock, 2KHz */
      SigInternal20KHz,       /* Device built-in clock, 20KHz */
      SigInternal200KHz,      /* Device built-in clock, 200KHz */
      SigInternal2MHz,        /* Device built-in clock, 2MHz */
   }

   public enum EventId
   {
      EvtDeviceRemoved = 0,  /* The device was removed from system */
      EvtDeviceReconnected,  /* The device is reconnected */
      EvtPropertyChanged,    /* Some properties of the device were changed */
      /*-----------------------------------------------------------------
      * AI events
      *-----------------------------------------------------------------*/
      EvtBufferedAiDataReady,
      EvtBufferedAiOverrun,
      EvtBufferedAiCacheOverflow,
      EvtBufferedAiStopped,

      /*-----------------------------------------------------------------
      * AO event IDs
      *-----------------------------------------------------------------*/
      EvtBufferedAoDataTransmitted,
      EvtBufferedAoUnderrun,
      EvtBufferedAoCacheEmptied,
      EvtBufferedAoTransStopped,
      EvtBufferedAoStopped,

      /*-----------------------------------------------------------------
      * DIO event IDs
      *-----------------------------------------------------------------*/
      EvtDiintChannel000, EvtDiintChannel001, EvtDiintChannel002, EvtDiintChannel003,
      EvtDiintChannel004, EvtDiintChannel005, EvtDiintChannel006, EvtDiintChannel007,
      EvtDiintChannel008, EvtDiintChannel009, EvtDiintChannel010, EvtDiintChannel011,
      EvtDiintChannel012, EvtDiintChannel013, EvtDiintChannel014, EvtDiintChannel015,
      EvtDiintChannel016, EvtDiintChannel017, EvtDiintChannel018, EvtDiintChannel019,
      EvtDiintChannel020, EvtDiintChannel021, EvtDiintChannel022, EvtDiintChannel023,
      EvtDiintChannel024, EvtDiintChannel025, EvtDiintChannel026, EvtDiintChannel027,
      EvtDiintChannel028, EvtDiintChannel029, EvtDiintChannel030, EvtDiintChannel031,
      EvtDiintChannel032, EvtDiintChannel033, EvtDiintChannel034, EvtDiintChannel035,
      EvtDiintChannel036, EvtDiintChannel037, EvtDiintChannel038, EvtDiintChannel039,
      EvtDiintChannel040, EvtDiintChannel041, EvtDiintChannel042, EvtDiintChannel043,
      EvtDiintChannel044, EvtDiintChannel045, EvtDiintChannel046, EvtDiintChannel047,
      EvtDiintChannel048, EvtDiintChannel049, EvtDiintChannel050, EvtDiintChannel051,
      EvtDiintChannel052, EvtDiintChannel053, EvtDiintChannel054, EvtDiintChannel055,
      EvtDiintChannel056, EvtDiintChannel057, EvtDiintChannel058, EvtDiintChannel059,
      EvtDiintChannel060, EvtDiintChannel061, EvtDiintChannel062, EvtDiintChannel063,
      EvtDiintChannel064, EvtDiintChannel065, EvtDiintChannel066, EvtDiintChannel067,
      EvtDiintChannel068, EvtDiintChannel069, EvtDiintChannel070, EvtDiintChannel071,
      EvtDiintChannel072, EvtDiintChannel073, EvtDiintChannel074, EvtDiintChannel075,
      EvtDiintChannel076, EvtDiintChannel077, EvtDiintChannel078, EvtDiintChannel079,
      EvtDiintChannel080, EvtDiintChannel081, EvtDiintChannel082, EvtDiintChannel083,
      EvtDiintChannel084, EvtDiintChannel085, EvtDiintChannel086, EvtDiintChannel087,
      EvtDiintChannel088, EvtDiintChannel089, EvtDiintChannel090, EvtDiintChannel091,
      EvtDiintChannel092, EvtDiintChannel093, EvtDiintChannel094, EvtDiintChannel095,
      EvtDiintChannel096, EvtDiintChannel097, EvtDiintChannel098, EvtDiintChannel099,
      EvtDiintChannel100, EvtDiintChannel101, EvtDiintChannel102, EvtDiintChannel103,
      EvtDiintChannel104, EvtDiintChannel105, EvtDiintChannel106, EvtDiintChannel107,
      EvtDiintChannel108, EvtDiintChannel109, EvtDiintChannel110, EvtDiintChannel111,
      EvtDiintChannel112, EvtDiintChannel113, EvtDiintChannel114, EvtDiintChannel115,
      EvtDiintChannel116, EvtDiintChannel117, EvtDiintChannel118, EvtDiintChannel119,
      EvtDiintChannel120, EvtDiintChannel121, EvtDiintChannel122, EvtDiintChannel123,
      EvtDiintChannel124, EvtDiintChannel125, EvtDiintChannel126, EvtDiintChannel127,
      EvtDiintChannel128, EvtDiintChannel129, EvtDiintChannel130, EvtDiintChannel131,
      EvtDiintChannel132, EvtDiintChannel133, EvtDiintChannel134, EvtDiintChannel135,
      EvtDiintChannel136, EvtDiintChannel137, EvtDiintChannel138, EvtDiintChannel139,
      EvtDiintChannel140, EvtDiintChannel141, EvtDiintChannel142, EvtDiintChannel143,
      EvtDiintChannel144, EvtDiintChannel145, EvtDiintChannel146, EvtDiintChannel147,
      EvtDiintChannel148, EvtDiintChannel149, EvtDiintChannel150, EvtDiintChannel151,
      EvtDiintChannel152, EvtDiintChannel153, EvtDiintChannel154, EvtDiintChannel155,
      EvtDiintChannel156, EvtDiintChannel157, EvtDiintChannel158, EvtDiintChannel159,
      EvtDiintChannel160, EvtDiintChannel161, EvtDiintChannel162, EvtDiintChannel163,
      EvtDiintChannel164, EvtDiintChannel165, EvtDiintChannel166, EvtDiintChannel167,
      EvtDiintChannel168, EvtDiintChannel169, EvtDiintChannel170, EvtDiintChannel171,
      EvtDiintChannel172, EvtDiintChannel173, EvtDiintChannel174, EvtDiintChannel175,
      EvtDiintChannel176, EvtDiintChannel177, EvtDiintChannel178, EvtDiintChannel179,
      EvtDiintChannel180, EvtDiintChannel181, EvtDiintChannel182, EvtDiintChannel183,
      EvtDiintChannel184, EvtDiintChannel185, EvtDiintChannel186, EvtDiintChannel187,
      EvtDiintChannel188, EvtDiintChannel189, EvtDiintChannel190, EvtDiintChannel191,
      EvtDiintChannel192, EvtDiintChannel193, EvtDiintChannel194, EvtDiintChannel195,
      EvtDiintChannel196, EvtDiintChannel197, EvtDiintChannel198, EvtDiintChannel199,
      EvtDiintChannel200, EvtDiintChannel201, EvtDiintChannel202, EvtDiintChannel203,
      EvtDiintChannel204, EvtDiintChannel205, EvtDiintChannel206, EvtDiintChannel207,
      EvtDiintChannel208, EvtDiintChannel209, EvtDiintChannel210, EvtDiintChannel211,
      EvtDiintChannel212, EvtDiintChannel213, EvtDiintChannel214, EvtDiintChannel215,
      EvtDiintChannel216, EvtDiintChannel217, EvtDiintChannel218, EvtDiintChannel219,
      EvtDiintChannel220, EvtDiintChannel221, EvtDiintChannel222, EvtDiintChannel223,
      EvtDiintChannel224, EvtDiintChannel225, EvtDiintChannel226, EvtDiintChannel227,
      EvtDiintChannel228, EvtDiintChannel229, EvtDiintChannel230, EvtDiintChannel231,
      EvtDiintChannel232, EvtDiintChannel233, EvtDiintChannel234, EvtDiintChannel235,
      EvtDiintChannel236, EvtDiintChannel237, EvtDiintChannel238, EvtDiintChannel239,
      EvtDiintChannel240, EvtDiintChannel241, EvtDiintChannel242, EvtDiintChannel243,
      EvtDiintChannel244, EvtDiintChannel245, EvtDiintChannel246, EvtDiintChannel247,
      EvtDiintChannel248, EvtDiintChannel249, EvtDiintChannel250, EvtDiintChannel251,
      EvtDiintChannel252, EvtDiintChannel253, EvtDiintChannel254, EvtDiintChannel255,

      EvtDiCosintPort000, EvtDiCosintPort001, EvtDiCosintPort002, EvtDiCosintPort003,
      EvtDiCosintPort004, EvtDiCosintPort005, EvtDiCosintPort006, EvtDiCosintPort007,
      EvtDiCosintPort008, EvtDiCosintPort009, EvtDiCosintPort010, EvtDiCosintPort011,
      EvtDiCosintPort012, EvtDiCosintPort013, EvtDiCosintPort014, EvtDiCosintPort015,
      EvtDiCosintPort016, EvtDiCosintPort017, EvtDiCosintPort018, EvtDiCosintPort019,
      EvtDiCosintPort020, EvtDiCosintPort021, EvtDiCosintPort022, EvtDiCosintPort023,
      EvtDiCosintPort024, EvtDiCosintPort025, EvtDiCosintPort026, EvtDiCosintPort027,
      EvtDiCosintPort028, EvtDiCosintPort029, EvtDiCosintPort030, EvtDiCosintPort031,

      EvtDiPmintPort000, EvtDiPmintPort001, EvtDiPmintPort002, EvtDiPmintPort003,
      EvtDiPmintPort004, EvtDiPmintPort005, EvtDiPmintPort006, EvtDiPmintPort007,
      EvtDiPmintPort008, EvtDiPmintPort009, EvtDiPmintPort010, EvtDiPmintPort011,
      EvtDiPmintPort012, EvtDiPmintPort013, EvtDiPmintPort014, EvtDiPmintPort015,
      EvtDiPmintPort016, EvtDiPmintPort017, EvtDiPmintPort018, EvtDiPmintPort019,
      EvtDiPmintPort020, EvtDiPmintPort021, EvtDiPmintPort022, EvtDiPmintPort023,
      EvtDiPmintPort024, EvtDiPmintPort025, EvtDiPmintPort026, EvtDiPmintPort027,
      EvtDiPmintPort028, EvtDiPmintPort029, EvtDiPmintPort030, EvtDiPmintPort031,

      EvtBufferedDiDataReady,
      EvtBufferedDiOverrun,
      EvtBufferedDiCacheOverflow,
      EvtBufferedDiStopped,

      EvtBufferedDoDataTransmitted,
      EvtBufferedDoUnderrun,
      EvtBufferedDoCacheEmptied,
      EvtBufferedDoTransStopped,
      EvtBufferedDoStopped,

      EvtReflectWdtOccured,
      /*-----------------------------------------------------------------
      * Counter/Timer event IDs
      *-----------------------------------------------------------------*/
      EvtCntTerminalCount0, EvtCntTerminalCount1, EvtCntTerminalCount2, EvtCntTerminalCount3,
      EvtCntTerminalCount4, EvtCntTerminalCount5, EvtCntTerminalCount6, EvtCntTerminalCount7,

      EvtCntOverCompare0, EvtCntOverCompare1, EvtCntOverCompare2, EvtCntOverCompare3,
      EvtCntOverCompare4, EvtCntOverCompare5, EvtCntOverCompare6, EvtCntOverCompare7,

      EvtCntUnderCompare0, EvtCntUnderCompare1, EvtCntUnderCompare2, EvtCntUnderCompare3,
      EvtCntUnderCompare4, EvtCntUnderCompare5, EvtCntUnderCompare6, EvtCntUnderCompare7,

      EvtCntEcOverCompare0, EvtCntEcOverCompare1, EvtCntEcOverCompare2, EvtCntEcOverCompare3,
      EvtCntEcOverCompare4, EvtCntEcOverCompare5, EvtCntEcOverCompare6, EvtCntEcOverCompare7,

      EvtCntEcUnderCompare0, EvtCntEcUnderCompare1, EvtCntEcUnderCompare2, EvtCntEcUnderCompare3,
      EvtCntEcUnderCompare4, EvtCntEcUnderCompare5, EvtCntEcUnderCompare6, EvtCntEcUnderCompare7,

      EvtCntOneShot0, EvtCntOneShot1, EvtCntOneShot2, EvtCntOneShot3,
      EvtCntOneShot4, EvtCntOneShot5, EvtCntOneShot6, EvtCntOneShot7,

      EvtCntTimer0, EvtCntTimer1, EvtCntTimer2, EvtCntTimer3,
      EvtCntTimer4, EvtCntTimer5, EvtCntTimer6, EvtCntTimer7,

      EvtCntPwmInOverflow0, EvtCntPwmInOverflow1, EvtCntPwmInOverflow2, EvtCntPwmInOverflow3,
      EvtCntPwmInOverflow4, EvtCntPwmInOverflow5, EvtCntPwmInOverflow6, EvtCntPwmInOverflow7,
   }

   [Flags]
   public enum PropertyAttribute
   {
      ReadOnly = 0,
      Writable = 1,
      Modal = 0,
      Nature = 2,
   }

   public enum PropertyId
   {
      /*-----------------------------------------------------------------
      * common property
      *-----------------------------------------------------------------*/
      CFG_Number,
      CFG_ComponentType,
      CFG_Description,
      CFG_Parent,
      CFG_ChildList,

      /*-----------------------------------------------------------------
      * component specified Property IDs -- group
      *-----------------------------------------------------------------*/
      CFG_DevicesNumber,
      CFG_DevicesHandle,

      /*-----------------------------------------------------------------
      * component specified Property IDs -- device
      *-----------------------------------------------------------------*/
      CFG_DeviceGroupNumber,
      CFG_DeviceProductID,
      CFG_DeviceBoardID,
      CFG_DeviceBoardVersion,
      CFG_DeviceDriverVersion,
      CFG_DeviceDllVersion,
      CFG_DeviceLocation,                       /* Reserved for later using */
      CFG_DeviceBaseAddresses,                  /* Reserved for later using */
      CFG_DeviceInterrupts,                     /* Reserved for later using */
      CFG_DeviceSupportedTerminalBoardTypes,    /* Reserved for later using */
      CFG_DeviceTerminalBoardType,              /* Reserved for later using */
      CFG_DeviceSupportedEvents,
      CFG_DeviceHotResetPreventable,            /* Reserved for later using */
      CFG_DeviceLoadingTimeInit,                /* Reserved for later using */
      CFG_DeviceWaitingForReconnect,
      CFG_DeviceWaitingForSleep,

      /*-----------------------------------------------------------------
      * component specified Property IDs -- AI, AO...
      *-----------------------------------------------------------------*/
      CFG_FeatureResolutionInBit,
      CFG_FeatureDataSize,
      CFG_FeatureDataMask,
      CFG_FeatureChannelNumberMax,
      CFG_FeatureChannelConnectionType,
      CFG_FeatureBurnDetectedReturnTypes,
      CFG_FeatureBurnoutDetectionChannels,
      CFG_FeatureOverallVrgType,
      CFG_FeatureVrgTypes,
      CFG_FeatureExtRefRange,
      CFG_FeatureExtRefAntiPolar,
      CFG_FeatureCjcChannels,
      CFG_FeatureChannelScanMethod,
      CFG_FeatureScanChannelStartBase,
      CFG_FeatureScanChannelCountBase,
      CFG_FeatureConvertClockSources,
      CFG_FeatureConvertClockRateRange,       /* Reserved for later using */
      CFG_FeatureScanClockSources,
      CFG_FeatureScanClockRateRange,         /* Reserved for later using */
      CFG_FeatureScanCountMax,               /* Reserved for later using */
      CFG_FeatureTriggersCount,
      CFG_FeatureTriggerSources,
      CFG_FeatureTriggerActions,
      CFG_FeatureTriggerDelayCountRange,
      CFG_FeatureTriggerSources1,            /* Reserved for later using */
      CFG_FeatureTriggerActions1,            /* Reserved for later using */
      CFG_FeatureTriggerDelayCountRange1,    /* Reserved for later using */

      CFG_ChannelCount,
      CFG_ConnectionTypeOfChannels,
      CFG_VrgTypeOfChannels,
      CFG_BurnDetectedReturnTypeOfChannels,
      CFG_BurnoutReturnValueOfChannels,
      CFG_ExtRefValueForUnipolar,         /* Reserved for later using */
      CFG_ExtRefValueForBipolar,          /* Reserved for later using */

      CFG_CjcChannel,
      CFG_CjcUpdateFrequency,             /* Reserved for later using */
      CFG_CjcValue,

      CFG_SectionDataCount,
      CFG_ConvertClockSource,
      CFG_ConvertClockRatePerChannel,
      CFG_ScanChannelStart,
      CFG_ScanChannelCount,
      CFG_ScanClockSource,                /* Reserved for later using */
      CFG_ScanClockRate,                  /* Reserved for later using */
      CFG_ScanCount,                      /* Reserved for later using */
      CFG_TriggerSource,
      CFG_TriggerSourceEdge,
      CFG_TriggerSourceLevel,
      CFG_TriggerDelayCount,
      CFG_TriggerAction,
      CFG_TriggerSource1,                 /* Reserved for later using */
      CFG_TriggerSourceEdge1,             /* Reserved for later using */
      CFG_TriggerSourceLevel1,            /* Reserved for later using */
      CFG_TriggerDelayCount1,             /* Reserved for later using */
      CFG_TriggerAction1,                 /* Reserved for later using */
      CFG_ParentSignalConnectionChannel,
      CFG_ParentCjcConnectionChannel,
      CFG_ParentControlPort,

      /*-----------------------------------------------------------------
      * component specified Property IDs -- DIO
      *-----------------------------------------------------------------*/
      CFG_FeaturePortsCount,
      CFG_FeaturePortsType,
      CFG_FeatureDiNoiseFilterOfChannels,
      CFG_FeatureDiNoiseFilterBlockTimeRange,     /* Reserved for later using */
      CFG_FeatureDiintTriggerEdges,
      CFG_FeatureDiintOfChannels,
      CFG_FeatureDiintGateOfChannels,
      CFG_FeatureDiCosintOfChannels,
      CFG_FeatureDiPmintOfChannels,
      CFG_FeatureDiSnapEventSources,
      CFG_FeatureDoFreezeSignalSources,            /* Reserved for later using */
      CFG_FeatureDoReflectWdtFeedIntervalRange,    /* Reserved for later using */

      CFG_FeatureDiPortScanMethod,                 /* Reserved for later using */
      CFG_FeatureDiConvertClockSources,            /* Reserved for later using */
      CFG_FeatureDiConvertClockRateRange,          /* Reserved for later using */
      CFG_FeatureDiScanClockSources,
      CFG_FeatureDiScanClockRateRange,             /* Reserved for later using */
      CFG_FeatureDiScanCountMax,
      CFG_FeatureDiTriggersCount,
      CFG_FeatureDiTriggerSources,
      CFG_FeatureDiTriggerActions,
      CFG_FeatureDiTriggerDelayCountRange,
      CFG_FeatureDiTriggerSources1,
      CFG_FeatureDiTriggerActions1,
      CFG_FeatureDiTriggerDelayCountRange1,

      CFG_FeatureDoPortScanMethod,                 /* Reserved for later using */
      CFG_FeatureDoConvertClockSources,            /* Reserved for later using */
      CFG_FeatureDoConvertClockRateRange,          /* Reserved for later using */
      CFG_FeatureDoScanClockSources,
      CFG_FeatureDoScanClockRateRange,             /* Reserved for later using */
      CFG_FeatureDoScanCountMax,
      CFG_FeatureDoTriggersCount,
      CFG_FeatureDoTriggerSources,
      CFG_FeatureDoTriggerActions,
      CFG_FeatureDoTriggerDelayCountRange,
      CFG_FeatureDoTriggerSources1,
      CFG_FeatureDoTriggerActions1,
      CFG_FeatureDoTriggerDelayCountRange1,

      CFG_DirectionOfPorts,
      CFG_DiDataMaskOfPorts,
      CFG_DoDataMaskOfPorts,

      CFG_DiNoiseFilterOverallBlockTime,              /* Reserved for later using */
      CFG_DiNoiseFilterEnabledChannels,
      CFG_DiintTriggerEdgeOfChannels,
      CFG_DiintGateEnabledChannels,
      CFG_DiCosintEnabledChannels,
      CFG_DiPmintEnabledChannels,
      CFG_DiPmintValueOfPorts,
      CFG_DoInitialStateOfPorts,                   /* Reserved for later using */
      CFG_DoFreezeEnabled,                         /* Reserved for later using */
      CFG_DoFreezeSignalState,                     /* Reserved for later using */
      CFG_DoReflectWdtFeedInterval,                /* Reserved for later using */
      CFG_DoReflectWdtLockValue,                   /* Reserved for later using */
      CFG_DiSectionDataCount,
      CFG_DiConvertClockSource,
      CFG_DiConvertClockRatePerPort,
      CFG_DiScanPortStart,
      CFG_DiScanPortCount,
      CFG_DiScanClockSource,
      CFG_DiScanClockRate,
      CFG_DiScanCount,
      CFG_DiTriggerAction,
      CFG_DiTriggerSource,
      CFG_DiTriggerSourceEdge,
      CFG_DiTriggerSourceLevel,                    /* Reserved for later using */
      CFG_DiTriggerDelayCount,
      CFG_DiTriggerAction1,
      CFG_DiTriggerSource1,
      CFG_DiTriggerSourceEdge1,
      CFG_DiTriggerSourceLevel1,                   /* Reserved for later using */
      CFG_DiTriggerDelayCount1,

      CFG_DoSectionDataCount,
      CFG_DoConvertClockSource,
      CFG_DoConvertClockRatePerPort,
      CFG_DoScanPortStart,
      CFG_DoScanPortCount,
      CFG_DoScanClockSource,
      CFG_DoScanClockRate,
      CFG_DoScanCount,
      CFG_DoTriggerAction,
      CFG_DoTriggerSource,
      CFG_DoTriggerSourceEdge,
      CFG_DoTriggerSourceLevel,                    /* Reserved for later using */
      CFG_DoTriggerDelayCount,
      CFG_DoTriggerAction1,
      CFG_DoTriggerSource1,
      CFG_DoTriggerSourceEdge1,
      CFG_DoTriggerSourceLevel1,                   /* Reserved for later using */
      CFG_DoTriggerDelayCount1,

      /*-----------------------------------------------------------------
      * component specified Property IDs -- Counter/Timer
      *-----------------------------------------------------------------*/
      /*common feature*/
      CFG_FeatureClkPolarities,
      CFG_FeatureGatePolarities,

      CFG_FeatureCapabilitiesOfCounter0 = CFG_FeatureGatePolarities + 3,
      CFG_FeatureCapabilitiesOfCounter1,
      CFG_FeatureCapabilitiesOfCounter2,
      CFG_FeatureCapabilitiesOfCounter3,
      CFG_FeatureCapabilitiesOfCounter4,
      CFG_FeatureCapabilitiesOfCounter5,
      CFG_FeatureCapabilitiesOfCounter6,
      CFG_FeatureCapabilitiesOfCounter7,

      /*primal counter features*/
      CFG_FeatureChipOperationModes = CFG_FeatureCapabilitiesOfCounter7 + 25,
      CFG_FeatureChipSignalCountingTypes,

      /*event counting features*/
      CFG_FeatureEcSignalCountingTypes,
      CFG_FeatureEcOverCompareIntCounters,
      CFG_FeatureEcUnderCompareIntCounters,

      /*timer/pulse features*/
      CFG_FeatureTmrCascadeGroups,

      /*frequency measurement features*/
      CFG_FeatureFmCascadeGroups,
      CFG_FeatureFmMethods,

      /*Primal counter properties */
      CFG_ChipOperationModeOfCounters = CFG_FeatureFmMethods + 7,
      CFG_ChipSignalCountingTypeOfCounters,
      CFG_ChipLoadValueOfCounters,
      CFG_ChipHoldValueOfCounters,
      CFG_ChipOverCompareValueOfCounters,
      CFG_ChipUnderCompareValueOfCounters,
      CFG_ChipOverCompareEnabledCounters,
      CFG_ChipUnderCompareEnabledCounters,

      /*Event counting properties*/
      CFG_EcOverCompareValueOfCounters,
      CFG_EcUnderCompareValueOfCounters,
      CFG_EcSignalCountingTypeOfCounters,

      /*frequency measurement properties*/
      CFG_FmMethodOfCounters,
      CFG_FmCollectionPeriodOfCounters,

      //##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
      // v1.1
      //##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
      CFG_DevicePrivateRegionLength,
      CFG_SaiAutoConvertClockRate,
      CFG_SaiAutoConvertChannelStart,
      CFG_SaiAutoConvertChannelCount,
      CFG_ExtPauseSignalEnabled,
      CFG_ExtPauseSignalPolarity,
      CFG_OrderOfChannels,
      CFG_InitialStateOfChannels,

      //##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
      // v1.2: new features & properties of counter
      //##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
      /*common features*/
      CFG_FeatureOutSignalTypes,

      /*primal counter features*/
      CFG_FeatureChipClkSourceOfCounter0,
      CFG_FeatureChipClkSourceOfCounter1,
      CFG_FeatureChipClkSourceOfCounter2,
      CFG_FeatureChipClkSourceOfCounter3,
      CFG_FeatureChipClkSourceOfCounter4,
      CFG_FeatureChipClkSourceOfCounter5,
      CFG_FeatureChipClkSourceOfCounter6,
      CFG_FeatureChipClkSourceOfCounter7,

      CFG_FeatureChipGateSourceOfCounter0,
      CFG_FeatureChipGateSourceOfCounter1,
      CFG_FeatureChipGateSourceOfCounter2,
      CFG_FeatureChipGateSourceOfCounter3,
      CFG_FeatureChipGateSourceOfCounter4,
      CFG_FeatureChipGateSourceOfCounter5,
      CFG_FeatureChipGateSourceOfCounter6,
      CFG_FeatureChipGateSourceOfCounter7,

      CFG_FeatureChipValueRegisters,

      /*one-shot features*/
      CFG_FeatureOsClkSourceOfCounter0,
      CFG_FeatureOsClkSourceOfCounter1,
      CFG_FeatureOsClkSourceOfCounter2,
      CFG_FeatureOsClkSourceOfCounter3,
      CFG_FeatureOsClkSourceOfCounter4,
      CFG_FeatureOsClkSourceOfCounter5,
      CFG_FeatureOsClkSourceOfCounter6,
      CFG_FeatureOsClkSourceOfCounter7,

      CFG_FeatureOsGateSourceOfCounter0,
      CFG_FeatureOsGateSourceOfCounter1,
      CFG_FeatureOsGateSourceOfCounter2,
      CFG_FeatureOsGateSourceOfCounter3,
      CFG_FeatureOsGateSourceOfCounter4,
      CFG_FeatureOsGateSourceOfCounter5,
      CFG_FeatureOsGateSourceOfCounter6,
      CFG_FeatureOsGateSourceOfCounter7,

      /*Pulse width measurement features*/
      CFG_FeaturePiCascadeGroups,

      /*common properties*/
      CFG_ClkPolarityOfCounters,
      CFG_GatePolarityOfCounters,
      CFG_OutSignalTypeOfCounters,

      /*Primal counter properties */
      CFG_ChipClkSourceOfCounters,
      CFG_ChipGateSourceOfCounters,

      /*one-shot properties*/
      CFG_OsClkSourceOfCounters,
      CFG_OsGateSourceOfCounters,
      CFG_OsDelayCountOfCounters,

      /*Timer pulse properties*/
      CFG_TmrFrequencyOfCounters,

      /*Pulse width modulation properties*/
      CFG_PoHiPeriodOfCounters,
      CFG_PoLoPeriodOfCounters,
   }

   public enum ErrorCode
   {
      /// <summary>
      /// The operation is completed successfully. 
      /// </summary>
      Success = 0,

      ///************************************************************************
      /// warning                                                              
      ///************************************************************************
      /// <summary>
      /// The interrupt resource is not available. 
      /// </summary>
      WarningIntrNotAvailable = unchecked((int)0xA0000000),

      /// <summary>
      /// The parameter is out of the range. 
      /// </summary>
      WarningParamOutOfRange = unchecked((int)0xA0000001),

      /// <summary>
      /// The property value is out of range. 
      /// </summary>
      WarningPropValueOutOfRange = unchecked((int)0xA0000002),

      /// <summary>
      /// The property value is not supported. 
      /// </summary>
      WarningPropValueNotSpted = unchecked((int)0xA0000003),

      /// <summary>
      /// The property value conflicts with the current state.
      /// </summary>
      WarningPropValueConflict = unchecked((int)0xA0000004),

      ///***********************************************************************
      /// error                                                                
      ///***********************************************************************
      /// <summary>
      /// The handle is NULL or its type doesn't match the required operation. 
      /// </summary>
      ErrorHandleNotValid = unchecked((int)0xE0000000),

      /// <summary>
      /// The parameter value is out of range.
      /// </summary>
      ErrorParamOutOfRange = unchecked((int)0xE0000001),

      /// <summary>
      /// The parameter value is not supported.
      /// </summary>
      ErrorParamNotSpted = unchecked((int)0xE0000002),

      /// <summary>
      /// The parameter value format is not the expected. 
      /// </summary>
      ErrorParamFmtUnexpted = unchecked((int)0xE0000003),

      /// <summary>
      /// Not enough memory is available to complete the operation. 
      /// </summary>
      ErrorMemoryNotEnough = unchecked((int)0xE0000004),

      /// <summary>
      /// The data buffer is null. 
      /// </summary>
      ErrorBufferIsNull = unchecked((int)0xE0000005),

      /// <summary>
      /// The data buffer is too small for the operation. 
      /// </summary>
      ErrorBufferTooSmall = unchecked((int)0xE0000006),

      /// <summary>
      /// The data length exceeded the limitation. 
      /// </summary>
      ErrorDataLenExceedLimit = unchecked((int)0xE0000007),

      /// <summary>
      /// The required function is not supported. 
      /// </summary>
      ErrorFuncNotSpted = unchecked((int)0xE0000008),

      /// <summary>
      /// The required event is not supported. 
      /// </summary>
      ErrorEventNotSpted = unchecked((int)0xE0000009),

      /// <summary>
      /// The required property is not supported. 
      /// </summary>
      ErrorPropNotSpted = unchecked((int)0xE000000A),

      /// <summary>
      /// The required property is read-only. 
      /// </summary>
      ErrorPropReadOnly = unchecked((int)0xE000000B),

      /// <summary>
      /// The specified property value conflicts with the current state.
      /// </summary>
      ErrorPropValueConflict = unchecked((int)0xE000000C),

      /// <summary>
      /// The specified property value is out of range.
      /// </summary>
      ErrorPropValueOutOfRange = unchecked((int)0xE000000D),

      /// <summary>
      /// The specified property value is not supported. 
      /// </summary>
      ErrorPropValueNotSpted = unchecked((int)0xE000000E),

      /// <summary>
      /// The handle hasn't own the privilege of the operation the user wanted. 
      /// </summary>
      ErrorPrivilegeNotHeld = unchecked((int)0xE000000F),

      /// <summary>
      /// The required privilege is not available because someone else had own it. 
      /// </summary>
      ErrorPrivilegeNotAvailable = unchecked((int)0xE0000010),

      /// <summary>
      /// The driver of specified device was not found. 
      /// </summary>
      ErrorDriverNotFound = unchecked((int)0xE0000011),

      /// <summary>
      /// The driver version of the specified device mismatched. 
      /// </summary>
      ErrorDriverVerMismatch = unchecked((int)0xE0000012),

      /// <summary>
      /// The loaded driver count exceeded the limitation. 
      /// </summary>
      ErrorDriverCountExceedLimit = unchecked((int)0xE0000013),

      /// <summary>
      /// The device is not opened. 
      /// </summary>
      ErrorDeviceNotOpened = unchecked((int)0xE0000014),

      /// <summary>
      /// The required device does not exist. 
      /// </summary>
      ErrorDeviceNotExist = unchecked((int)0xE0000015),

      /// <summary>
      /// The required device is unrecognized by driver. 
      /// </summary>
      ErrorDeviceUnrecognized = unchecked((int)0xE0000016),

      /// <summary>
      /// The configuration data of the specified device is lost or unavailable. 
      /// </summary>
      ErrorConfigDataLost = unchecked((int)0xE0000017),

      /// <summary>
      /// The function is not initialized and can't be started. 
      /// </summary>
      ErrorFuncNotInited = unchecked((int)0xE0000018),

      /// <summary>
      /// The function is busy. 
      /// </summary>
      ErrorFuncBusy = unchecked((int)0xE0000019),

      /// <summary>
      /// The interrupt resource is not available. 
      /// </summary>
      ErrorIntrNotAvailable = unchecked((int)0xE000001A),

      /// <summary>
      /// The DMA channel is not available. 
      /// </summary>
      ErrorDmaNotAvailable = unchecked((int)0xE000001B),

      /// <summary>
      /// Time out when reading/writing the device. 
      /// </summary>
      ErrorDeviceIoTimeOut = unchecked((int)0xE000001C),

      /// <summary>
      /// The given signature does not match with the device current one.
      /// </summary>
      ErrorSignatureNotMatch = unchecked((int)0xE000001D),

      /// <summary>
      /// Undefined error 
      /// </summary>
      ErrorUndefined = unchecked((int)0xE000FFFF),
   };

   // Advantech CardType ID 
   public enum ProductId
   {
      BD_DEMO = 0x00,		// demo board
      BD_PCL818 = 0x05,		// PCL-818 board
      BD_PCL818H = 0x11,	// PCL-818H
      BD_PCL818L = 0x21,	// PCL-818L
      BD_PCL818HG = 0x22,	// PCL-818HG
      BD_PCL818HD = 0x2b,	// PCL-818HD
      BD_PCM3718 = 0x37,	// PCM-3718
      BD_PCM3724 = 0x38,	// PCM-3724
      BD_PCM3730 = 0x5a,	// PCM-3730
      BD_PCI1750 = 0x5e,	// PCI-1750
      BD_PCI1751 = 0x5f,	// PCI-1751
      BD_PCI1710 = 0x60,	// PCI-1710
      BD_PCI1712 = 0x61,	// PCI-1712
      BD_PCI1710HG = 0x67,	// PCI-1710HG
      BD_PCI1711 = 0x73,	// PCI-1711
      BD_PCI1711L = 0x75,	// PCI-1711L 
      BD_PCI1713 = 0x68,	// PCI-1713
      BD_PCI1753 = 0x69,	// PCI-1753
      BD_PCI1760 = 0x6a,	// PCI-1760
      BD_PCI1720 = 0x6b,	// PCI-1720
      BD_PCM3718H = 0x6d,	// PCM-3718H
      BD_PCM3718HG = 0x6e,	// PCM-3718HG
      BD_PCI1716 = 0x74,	// PCI-1716
      BD_PCI1731 = 0x75,	// PCI-1731
      BD_PCI1754 = 0x7b,	// PCI-1754
      BD_PCI1752 = 0x7c,	// PCI-1752
      BD_PCI1756 = 0x7d,	// PCI-1756
      BD_PCM3725 = 0x7f,	// PCM-3725
      BD_PCI1762 = 0x80,	// PCI-1762
      BD_PCI1721 = 0x81,	// PCI-1721
      BD_PCI1761 = 0x82,	// PCI-1761
      BD_PCI1723 = 0x83,	// PCI-1723
      BD_PCI1730 = 0x87,	// PCI-1730
      BD_PCI1733 = 0x88,	// PCI-1733
      BD_PCI1734 = 0x89,	// PCI-1734
      BD_PCI1710L = 0x90,	// PCI-1710L
      BD_PCI1710HGL = 0x91,// PCI-1710HGL
      BD_PCM3712 = 0x93,	// PCM-3712
      BD_PCM3723 = 0x94,	// PCM-3723
      BD_PCI1780 = 0x95,	// PCI-1780
      BD_CPCI3756 = 0x96,	// CPCI-3756
      BD_PCI1755 = 0x97,	// PCI-1755
      BD_PCI1714 = 0x98,	// PCI-1714
      BD_PCI1757 = 0x99,	// PCI-1757
      BD_MIC3716 = 0x9A,	// MIC-3716
      BD_MIC3761 = 0x9B,	// MIC-3761
      BD_MIC3753 = 0x9C,		// MIC-3753
      BD_MIC3780 = 0x9D,		// MIC-3780
      BD_PCI1724 = 0x9E,		// PCI-1724
      BD_PCI1758UDI = 0xA3,	// PCI-1758UDI
      BD_PCI1758UDO = 0xA4,	// PCI-1758UDO
      BD_PCI1747 = 0xA5,		// PCI-1747
      BD_PCM3780 = 0xA6,		// PCM-3780 
      BD_MIC3747 = 0xA7,		// MIC-3747
      BD_PCI1758UDIO = 0xA8,	// PCI-1758UDIO
      BD_PCI1712L = 0xA9,		// PCI-1712L
      BD_PCI1763UP = 0xAC,	   // PCI-1763UP
      BD_PCI1736UP = 0xAD,	   // PCI-1736UP
      BD_PCI1714UL = 0xAE,	   // PCI-1714UL
      BD_MIC3714 = 0xAF,		// MIC-3714
      BD_PCM3718HO = 0xB1,	   // PCM-3718HO
      BD_PCI1741U = 0xB3,		// PCI-1741U
      BD_MIC3723 = 0xB4,		// MIC-3723 
      BD_PCI1718HDU = 0xB5,	// PCI-1718HDU
      BD_MIC3758DIO = 0xB6,	// MIC-3758DIO
      BD_PCI1727U = 0xB7,		// PCI-1727U
      BD_PCI1718HGU = 0xB8,	// PCI-1718HGU
      BD_PCI1715U = 0xB9,		// PCI-1715U
      BD_PCI1716L = 0xBA,		// PCI-1716L
      BD_PCI1735U = 0xBB,		// PCI-1735U
      BD_USB4711 = 0xBC,		// USB4711
      BD_PCI1737U = 0xBD,		// PCI-1737U
      BD_PCI1739U = 0xBE,		// PCI-1739U
      BD_PCI1742U = 0xC0,		// PCI-1742U
      BD_USB4718 = 0xC6,		// USB-4718
      BD_MIC3755 = 0xC7,		// MIC3755
      BD_USB4761 = 0xC8,		// USB4761
      BD_PCI1784 = 0XCC,		// PCI-1784
      BD_USB4716 = 0xCD,		// USB4716
      BD_PCI1752U = 0xCE,		// PCI-1752U
      BD_PCI1752USO = 0xCF,	// PCI-1752USO
      BD_USB4751 = 0xD0,		// USB4751
      BD_USB4751L = 0xD1,		// USB4751L
      BD_USB4750 = 0xD2,		// USB4750
      BD_MIC3713 = 0xD3,		// MIC-3713
      BD_USB4711A = 0xD8,		// USB4711A
      BD_PCM3753P = 0xD9,		// PCM3753P
      BD_PCM3784 = 0xDA,		// PCM3784
      BD_PCM3761I = 0xDB,     // PCM-3761I
      BD_MIC3751 = 0xDC,      // MIC-3751
      BD_PCM3730I = 0xDD,     // PCM-3730I
      BD_PCM3813I = 0xE0,     // PCM-3813I
      BD_PCIE1744 = 0xE1,     //PCIE-1744
      BD_PCI1730U = 0xE2, 	   // PCI-1730U
      BD_PCI1760U = 0xE3,	   //PCI-1760U
      BD_MIC3720 = 0xE4,	   //MIC-3720
      BD_PCM3810I = 0xE9,     // PCM-3810I
      BD_USB4702 = 0xEA,      // USB4702
      BD_USB4704 = 0xEB,      // USB4704
      BD_PCM3810I_HG = 0xEC,  // PCM-3810I_HG
      BD_PCI1713U = 0xED,		// PCI-1713U 

      // !!!BioDAQ only Product ID starts from here!!!
      BD_PCI1706 = 0x800,
   }

#endregion

#region Bionic DAQ Wrapped classes

   public abstract class BDaqModule
   {
      protected IntPtr m_moduleHandle;
      protected BDaqProperty m_property;
      protected BDaqEvent m_event;

      protected BDaqModule(IntPtr moduleHandle)
      {
         m_moduleHandle = moduleHandle;
      }

      public IntPtr Handle
      {
         get { return m_moduleHandle; }
         protected set { m_moduleHandle = value; }
      }
      public BDaqProperty Property
      {
         get
         {
            if (m_property == null)
            {
               m_property = new BDaqModuleProperty(this);
            }
            return m_property;
         }
      }
      public BDaqEvent Event
      {
         get
         {
            if (m_event == null)
            {
               m_event = new BDaqModuleEvent(this);
            }
            return m_event;
         }
      }

      #region internal used only
      private class BDaqModuleProperty : BDaqProperty
      {
         public BDaqModuleProperty(BDaqModule module) : base(module) { }
      }
      private class BDaqModuleEvent : BDaqEvent
      {
         public BDaqModuleEvent(BDaqModule module) : base(module) { }
      }
      #endregion
   }

   public abstract class BDaqEvent
   {
      protected BDaqModule m_parent;
      internal BDaqEvent(BDaqModule parent)
      {
         m_parent = parent;
      }
      public ErrorCode GetHandle(EventId id, out WaitHandle eventHandle)
      {
         eventHandle = null;
         IntPtr w32Handle;
         ErrorCode ret = BDaqApi.AdxEventGetHandle(m_parent.Handle, id, out w32Handle);
         if (ret == ErrorCode.Success)
         {
            eventHandle = new AutoResetEvent(false);
            eventHandle.SafeWaitHandle = new SafeWaitHandle(w32Handle, false);
         } 
         return ret;
      }
      public unsafe int GetStatus(EventId id)
      {
         int statusLParam = 0;
         BDaqApi.AdxEventGetLastStatus(m_parent.Handle, id, &statusLParam, null);
         return statusLParam;
      }
      public unsafe int GetStatus(EventId id, out int statusRParam)
      {
         int statusLParam = 0;
         statusRParam = 0;
         fixed (int* rp = &statusRParam)
         {
            BDaqApi.AdxEventGetLastStatus(m_parent.Handle, id, &statusLParam, rp);
         }
         return statusLParam;
      }
      public void ClearFlag(EventId id, int flagLParam, int flagRParam)
      {
         BDaqApi.AdxEventClearFlag(m_parent.Handle, id, flagLParam, flagRParam);
      }
   }

   public abstract class BDaqProperty
   {
      protected BDaqModule m_parent;
      internal BDaqProperty(BDaqModule parent)
      {
         m_parent = parent;
      }
      public int NotifyPropertyChanged = 0; // whether or not notify the others the property was changed.

      #region helper method to get the attribute of a property
      public unsafe int GetLength(PropertyId id)
      {
         int dataLength;
         BDaqApi.AdxPropertyRead(m_parent.Handle, id, 0, null, &dataLength, null);
         return dataLength;
      }
      public unsafe PropertyAttribute GetAttribute(PropertyId id)
      {
         int propAttr = 0;
         BDaqApi.AdxPropertyRead(m_parent.Handle, id, 0, null, null, &propAttr);
         return (PropertyAttribute)propAttr;
      }
      #endregion

      #region get single-value property
      public unsafe ErrorCode Get(PropertyId id, out int data)
      {
         fixed (void* p = &data)
         {
            return BDaqApi.AdxPropertyRead(m_parent.Handle, id, sizeof(int), p, null, null);
         }
      }
      public unsafe ErrorCode Get(PropertyId id, out double data)
      {
         fixed (void* p = &data)
         {
            return BDaqApi.AdxPropertyRead(m_parent.Handle, id, sizeof(double), p, null, null);
         }
      }
      public unsafe ErrorCode Get(PropertyId id, out MathInterval data)
      {
         fixed (void* p = &data)
         {
            return BDaqApi.AdxPropertyRead(m_parent.Handle, id, (int)Marshal.SizeOf(typeof(MathInterval)), p, null, null);
         }
      }
      #endregion

      #region get array type property
      public unsafe ErrorCode Get(PropertyId id, int count, byte[] data)
      {
         fixed (void* p = &data[0])
         {
            return BDaqApi.AdxPropertyRead(m_parent.Handle, id, count * sizeof(byte), p, null, null);
         }
      }
      public unsafe ErrorCode Get(PropertyId id, int count, int[] data)
      {
         fixed (void* p = &data[0])
         {
            return BDaqApi.AdxPropertyRead(m_parent.Handle, id, count * sizeof(int), p, null, null);
         }
      }
      public unsafe ErrorCode Get(PropertyId id, int count, double[] data)
      {
         fixed (void* p = &data[0])
         {
            return BDaqApi.AdxPropertyRead(m_parent.Handle, id, count * sizeof(double), p, null, null);
         }
      }
      public unsafe ErrorCode Get(PropertyId id, int count, char[] data)
      {
         fixed (void* p = &data[0])
         {
            return BDaqApi.AdxPropertyRead(m_parent.Handle, id, count * sizeof(char), p, null, null);
         }
      }
      #endregion

      #region set single-value property
      public unsafe ErrorCode Set(PropertyId id, int data)
      {
         return BDaqApi.AdxPropertyWrite(m_parent.Handle, id, sizeof(int), (void*)&data, NotifyPropertyChanged);
      }
      public unsafe ErrorCode Set(PropertyId id, double data)
      {
         return BDaqApi.AdxPropertyWrite(m_parent.Handle, id, sizeof(double), (void*)&data, NotifyPropertyChanged);
      }
      public unsafe ErrorCode Set(PropertyId id, MathInterval data)
      {
         return BDaqApi.AdxPropertyWrite(m_parent.Handle, id, (int)Marshal.SizeOf(typeof(MathInterval)), (void*)&data, NotifyPropertyChanged);
      }
      #endregion

      #region set array type property
      public unsafe ErrorCode Set(PropertyId id, int count, byte[] data)
      {
         fixed (void* p = &data[0])
         {
            return BDaqApi.AdxPropertyWrite(m_parent.Handle, id, count * sizeof(byte), p, NotifyPropertyChanged);
         }
      }
      public unsafe ErrorCode Set(PropertyId id, int count, int[] data)
      {
         fixed (void* p = &data[0])
         {
            return BDaqApi.AdxPropertyWrite(m_parent.Handle, id, count * sizeof(int), p, NotifyPropertyChanged);
         }
      }
      public unsafe ErrorCode Set(PropertyId id, int count, double[] data)
      {
         fixed (void* p = &data[0])
         {
            return BDaqApi.AdxPropertyWrite(m_parent.Handle, id, count * sizeof(double), p, NotifyPropertyChanged);
         }
      }
      public unsafe ErrorCode Set(PropertyId id, int count, char[] data)
      {
         fixed (void* p = &data[0])
         {
            return BDaqApi.AdxPropertyWrite(m_parent.Handle, id, count * sizeof(char), p, NotifyPropertyChanged);
         }
      }
      #endregion
   }

   public abstract class BDaqAi : BDaqModule
   {
      #region instant AI single channel methods
      public unsafe ErrorCode Read(int channel, out double dataScaled)
      {
         fixed (void* s = &dataScaled)
         {
            return BDaqApi.AdxAiReadSamples(this.Handle, channel, 1, null, (double*)s);
         }
      }
      // for the device whose raw data is in 16bits format
      public unsafe ErrorCode Read(int channel, out short dataRaw)
      {
         fixed (void* r = &dataRaw)
         {
            return BDaqApi.AdxAiReadSamples(this.Handle, channel, 1, r, null);
         }
      }
      // for the device whose raw data is in 32bits format
      public unsafe ErrorCode Read(int channel, out int dataRaw)
      {
         fixed (void* r = &dataRaw)
         {
            return BDaqApi.AdxAiReadSamples(this.Handle, channel, 1, r, null);
         }
      }
      #endregion

      #region instant AI multi channel methods
      public unsafe ErrorCode Read(int chStart, int chCount, double[] dataScaled)
      {
         fixed (void* s = dataScaled)
         {
            return BDaqApi.AdxAiReadSamples(this.Handle, chStart, chCount, null, (double*)s);
         }
      }
      // for the device whose raw data is in 16bits format
      public unsafe ErrorCode Read(int chStart, int chCount, short[] dataRaw, double[] dataScaled)
      {
         fixed (void* r = dataRaw, s = dataScaled)
         {
            return BDaqApi.AdxAiReadSamples(this.Handle, chStart, chCount, r, (double*)s);
         }
      }
      // for the device whose raw data is in 32bits format
      public unsafe ErrorCode Read(int chStart, int chCount, int[] dataRaw, double[] dataScaled)
      {
         fixed (void* r = dataRaw, s = dataScaled)
         {
            return BDaqApi.AdxAiReadSamples(this.Handle, chStart, chCount, r, (double*)s);
         }
      }
      #endregion

      #region buffered AI methods
      public ErrorCode BfdAiPrepare(int dataCount, out IntPtr dataRaw)
      {
         return BDaqApi.AdxBufferedAiPrepare(this.Handle, dataCount, out dataRaw);
      }
      public ErrorCode BfdAiRunOnce(bool asynchronous)
      {
         return BDaqApi.AdxBufferedAiRunOnce(this.Handle, asynchronous ? 1 : 0);
      }
      public ErrorCode BfdAiRun()
      {
         return BDaqApi.AdxBufferedAiRun(this.Handle);
      }
      public unsafe ErrorCode BfdAiScaleData(IntPtr dataRaw, double[] dataScaled, int dataCount, ref int chOffset)
      {
         fixed (double* ptr = dataScaled)
         {
            return BDaqApi.AdxBufferedAiScaleData(this.Handle, (void*)dataRaw, ptr, dataCount, ref chOffset);
         }
      }
      public unsafe ErrorCode BfdAiScaleData(void* dataRaw, double* dataScaled, int dataCount, ref int chOffset)
      {
         return BDaqApi.AdxBufferedAiScaleData(this.Handle, dataRaw, dataScaled, dataCount, ref chOffset);
      }
      public ErrorCode BfdAiStop()
      {
         return BDaqApi.AdxBufferedAiStop(this.Handle);
      }
      public ErrorCode BfdAiRelease()
      {
         return BDaqApi.AdxBufferedAiRelease(this.Handle);
      }
      #endregion

      #region internal used only
      internal BDaqAi(IntPtr aiHandle)
         : base(aiHandle)
      {

      }
      #endregion
   }

   public abstract class BDaqAo : BDaqModule
   {
      #region instant AO single channel methods
      public unsafe ErrorCode Write(int channel, double dataScaled)
      {
         return BDaqApi.AdxAoWriteSamples(this.Handle, channel, 1, null, (double*)&dataScaled);
      }
      // for the device whose raw data is in 16bits format
      public unsafe ErrorCode Write(int channel, short dataRaw)
      {
         return BDaqApi.AdxAoWriteSamples(this.Handle, channel, 1, (void*)&dataRaw, null);
      }
      // for the device whose raw data is in 32bits format
      public unsafe ErrorCode Write(int channel, int dataRaw)
      {
         return BDaqApi.AdxAoWriteSamples(this.Handle, channel, 1, (void*)&dataRaw, null);
      }
      #endregion

      #region instant AO multi channel methods
      public unsafe ErrorCode Write(int chStart, int chCount, double[] dataScaled)
      {
         fixed (void* s = dataScaled)
         {
            return BDaqApi.AdxAoWriteSamples(this.Handle, chStart, chCount, null, (double*)s);
         }
      }
      // for the device whose raw data is in 16bits format
      public unsafe ErrorCode Write(int chStart, int chCount, short[] dataRaw)
      {
         fixed (void* r = dataRaw)
         {
            return BDaqApi.AdxAoWriteSamples(this.Handle, chStart, chCount, r, null);
         }
      }
      // for the device whose raw data is in 32bits format
      public unsafe ErrorCode Write(int chStart, int chCount, int[] dataRaw)
      {
         fixed (void* r = dataRaw)
         {
            return BDaqApi.AdxAoWriteSamples(this.Handle, chStart, chCount, r, null);
         }
      }
      #endregion

      #region buffered AO methods
      public ErrorCode BfdAoPrepare(int dataCount, out IntPtr dataRaw)
      {
         return BDaqApi.AdxBufferedAoPrepare(this.Handle, dataCount, out dataRaw);
      }
      public ErrorCode BfdAoRunOnce(bool asynchronous)
      {
         return BDaqApi.AdxBufferedAoRunOnce(this.Handle, asynchronous ? 1 : 0);
      }
      public ErrorCode BfdAoRun()
      {
         return BDaqApi.AdxBufferedAoRun(this.Handle);
      }
      public unsafe ErrorCode BfdAoScaleData(double[] dataScaled, IntPtr dataRaw, int dataCount, ref int chOffset)
      {
         fixed (double* ptr = dataScaled)
         {
            return BDaqApi.AdxBufferedAoScaleData(this.Handle, ptr, (void*)dataRaw, dataCount, ref chOffset);
         }
      }
      public unsafe ErrorCode BfdAoScaleData(double* dataScaled, void* dataRaw, int dataCount, ref int chOffset)
      {
         return BDaqApi.AdxBufferedAoScaleData(this.Handle, dataScaled, dataRaw, dataCount, ref chOffset);
      }
      public ErrorCode BfdAoStop(int action)
      {
         return BDaqApi.AdxBufferedAoStop(this.Handle, action);
      }
      public ErrorCode BfdAoRelease()
      {
         return BDaqApi.AdxBufferedAoRelease(this.Handle);
      }
      #endregion

      #region internal used only
      internal BDaqAo(IntPtr aoHandle)
         : base(aoHandle)
      {

      }
      #endregion
   }

   public abstract class BDaqDio : BDaqModule
   {
      #region Instant DI/O methods
      public unsafe ErrorCode DiRead(int port, out byte data)
      {
         fixed (byte* p = &data)
         {
            return BDaqApi.AdxDiReadPorts(this.Handle, port, 1, p);
         }
      }
      public unsafe ErrorCode DiRead(int portStart, int portCount, byte[] data)
      {
         fixed (byte* p = data)
         {
            return BDaqApi.AdxDiReadPorts(this.Handle, portStart, portCount, p);
         }
      }

      public unsafe ErrorCode DoWrite(int port, byte data)
      {
         return BDaqApi.AdxDoWritePorts(this.Handle, port, 1, &data);
      }
      public unsafe ErrorCode DoWrite(int portStart, int portCount, byte[] data)
      {
         fixed (byte* p = data)
         {
            return BDaqApi.AdxDoWritePorts(this.Handle, portStart, portCount, p);
         }
      }

      public unsafe ErrorCode DoRead(int port, out byte data)
      {
         fixed (byte* p = &data)
         {
            return BDaqApi.AdxDoReadBackPorts(this.Handle, port, 1, p);
         }
      }
      public unsafe ErrorCode DoRead(int portStart, int portCount, byte[] data)
      {
         fixed (byte* p = data)
         {
            return BDaqApi.AdxDoReadBackPorts(this.Handle, portStart, portCount, p);
         }
      }
      #endregion

      #region DI snap methods
      public ErrorCode DiSnapStart(EventId id, int portStart, int portCount, out IntPtr buffer)
      {
         return BDaqApi.AdxDiSnapStart(this.Handle, id, portStart, portCount, out buffer);
      }
      public ErrorCode DiSnapStop(EventId id)
      {
         return BDaqApi.AdxDiSnapStop(this.Handle, id);
      }
      #endregion

      #region Buffered DI methods
      public ErrorCode BfdDiPrepare(int dataCount, out IntPtr data)
      {
         return BDaqApi.AdxBufferedDiPrepare(this.Handle, dataCount, out data);
      }
      public ErrorCode BfdDiRunOnce(bool asynchronous)
      {
         return BDaqApi.AdxBufferedDiRunOnce(this.Handle, asynchronous ? 1 : 0);
      }
      public ErrorCode BfdDiRun()
      {
         return BDaqApi.AdxBufferedDiRun(this.Handle);
      }
      public ErrorCode BfdDiStop()
      {
         return BDaqApi.AdxBufferedDiStop(this.Handle);
      }
      public ErrorCode BfdDiRelease()
      {
         return BDaqApi.AdxBufferedDiRelease(this.Handle);
      }
      #endregion

      #region Buffered DO methods
      public ErrorCode BfdDoPrepare(int dataCount, out IntPtr data)
      {
         return BDaqApi.AdxBufferedDoPrepare(this.Handle, dataCount, out data);
      }
      public ErrorCode BfdDoRunOnce(bool asynchronous)
      {
         return BDaqApi.AdxBufferedDoRunOnce(this.Handle, asynchronous ? 1 : 0);
      }
      public ErrorCode BfdDoRun()
      {
         return BDaqApi.AdxBufferedDoRun(this.Handle);
      }
      public ErrorCode BfdDoStop(int action)
      {
         return BDaqApi.AdxBufferedDoStop(this.Handle, action);
      }
      public ErrorCode BfdDoRelease()
      {
         return BDaqApi.AdxBufferedDoRelease(this.Handle);
      }
      #endregion

      #region internal used only
      internal BDaqDio(IntPtr dioHandle)
         : base(dioHandle)
      {

      }
      #endregion
   }

   public abstract class BDaqCntr : BDaqModule
   {
      #region Common methods
      public ErrorCode Reset(int cntrStart, int cntrCount)
      {
         return BDaqApi.AdxCounterReset(this.Handle, cntrStart, cntrCount);
      }
      #endregion

      #region Event Counting methods
      public ErrorCode EventCountStart(int cntrStart, int cntrCount)
      {
         return BDaqApi.AdxEventCountStart(this.Handle, cntrStart, cntrCount);
      }
      public unsafe ErrorCode EventCountRead(int cntr, out int cntrValue)
      {
         fixed (int* p = &cntrValue)
         {
            return BDaqApi.AdxEventCountRead(this.Handle, cntr, 1, p);
         }
      }
      public unsafe ErrorCode EventCountRead(int cntrStart, int cntrCount, int[] cntrValue)
      {
         fixed (int* p = cntrValue)
         {
            return BDaqApi.AdxEventCountRead(this.Handle, cntrStart, cntrCount, p);
         }
      }
      #endregion

      #region One-Shot methods
      public ErrorCode OneShotStart(int cntrStart, int cntrCount)
      {
         return BDaqApi.AdxOneShotStart(this.Handle, cntrStart, cntrCount);
      }
      #endregion

      #region Timer/Pulse methods
      public ErrorCode TimerPulseStart(int cntrStart, int cntrCount)
      {
         return BDaqApi.AdxTimerPulseStart(this.Handle, cntrStart, cntrCount);
      }
      #endregion

      #region Frequency Measurement methods
      public ErrorCode FreqMeasureStart(int cntrStart, int cntrCount)
      {
         return BDaqApi.AdxFrequencyMeasureStart(this.Handle, cntrStart, cntrCount);
      }
      public unsafe ErrorCode FreqMeasureRead(int cntr, out double frequency)
      {
         fixed (double* p = &frequency)
         {
            return BDaqApi.AdxFrequencyMeasureRead(this.Handle, cntr, 1, p);
         }
      }
      public unsafe ErrorCode FreqMeasureRead(int cntrStart, int cntrCount, double[] frequency)
      {
         fixed (double* p = frequency)
         {
            return BDaqApi.AdxFrequencyMeasureRead(this.Handle, cntrStart, cntrCount, p);
         }
      }
      #endregion

      #region Pulse width measurement methods
      public ErrorCode PwmInStart(int cntrStart, int groupCount)
      {
         return BDaqApi.AdxPwmInStart(this.Handle, cntrStart, groupCount);
      }
      public unsafe ErrorCode PwmInRead(int cntr, out double hiPeriod, out double lowPeriod)
      {
         fixed (double* hi = &hiPeriod)
         {
            fixed (double* lo = &lowPeriod)
            {
               return BDaqApi.AdxPwmInRead(this.Handle, cntr, 1, hi, lo);
            }
         }
      }
      public unsafe ErrorCode PwmInRead(int cntrStart, int groupCount, double[] hiPeriod, double[] lowPeriod)
      {
         fixed (double* hi = hiPeriod)
         {
            fixed (double* lo = lowPeriod)
            {
               return BDaqApi.AdxPwmInRead(this.Handle, cntrStart, groupCount, hi, lo);
            }
         }
      }
      #endregion

      #region Pulse width modulation methods
      public ErrorCode PwmOutStart(int cntrStart, int cntrCount)
      {
         return BDaqApi.AdxPwmOutStart(this.Handle, cntrStart, cntrCount);
      }
      #endregion

      #region internal used only
      internal BDaqCntr(IntPtr cntrHandle)
         : base(cntrHandle)
      {

      }
      #endregion
   }

   public class BDaqDevice : BDaqModule, IDisposable
   {
      #region Methods
      // Open device by device number
      public static ErrorCode Open(int deviceNumber, AccessMode mode, out BDaqDevice device)
      {
         ErrorCode ret = ErrorCode.Success;
         // 'Singleton': all devices which own same device number and access mode
         //  will reference to the same object.
         int deviceKey = GetDeviceKey(deviceNumber, mode);
         lock (s_cachedDevices)
         {
            device = null;
            if (s_cachedDevices.TryGetValue(deviceKey, out device))
            {
               ++device.m_refCount;
            }
            else
            {
               IntPtr deviceHandle = IntPtr.Zero;
               ret = BDaqApi.AdxDeviceOpen(deviceNumber, mode, out deviceHandle);
               if (ret == ErrorCode.Success)
               {
                  device = new BDaqDevice(deviceHandle, deviceNumber, mode);
                  device.m_refCount = 1;
                  s_cachedDevices[deviceKey] = device;
               }
            }
         }
         return ret;
      }

      public static ErrorCode Open(string deviceDescription, AccessMode mode, out BDaqDevice device)
      {
         device = null;
         if (deviceDescription == null || deviceDescription.Length == 0)
         {
            return ErrorCode.ErrorBufferIsNull;
         }
         // remove the c-terminator char at the tail.
         char[] cterm = { '\0' };
         string devDescTrimed = deviceDescription.Trim(cterm);

         StringBuilder curDevDesc = new StringBuilder(256);
         int  deviceNumber= -1;
         int deviceIndex = 0;
         int subDevCount = 0;
         while( true )
         {
            ErrorCode ret = BDaqApi.AdxDeviceGetLinkageInfo(-1, deviceIndex, out deviceNumber, curDevDesc, out subDevCount);
            if ( deviceNumber == -1 )
            {
               // no more device
               return ErrorCode.ErrorDeviceNotExist;
            }
            if (ret == ErrorCode.Success && curDevDesc.ToString() == devDescTrimed)
            {
               return Open(deviceNumber, mode, out device);
            }
            ++deviceIndex;
         }
      }
      public ErrorCode GetModule(int index, out BDaqAi ai)
      {
         BDaqModule module;
         ErrorCode ret = GetModule(ModuleType.DaqAi, index, out module);
         ai = module as BDaqAi;
         return ret;
      }
      public ErrorCode GetModule(int index, out BDaqAo ao)
      {
         BDaqModule module;
         ErrorCode ret = GetModule(ModuleType.DaqAo, index, out module);
         ao = module as BDaqAo;
         return ret;
      }
      public ErrorCode GetModule(int index, out BDaqDio dio)
      {
         BDaqModule module;
         ErrorCode ret = GetModule(ModuleType.DaqDio, index, out module);
         dio = module as BDaqDio;
         return ret;
      }
      public ErrorCode GetModule(int index, out BDaqCntr cntr)
      {
         BDaqModule module;
         ErrorCode ret = GetModule(ModuleType.DaqCounter, index, out module);
         cntr = module as BDaqCntr;
         return ret;
      }
      public ErrorCode GetModule(ModuleType type, int index, out BDaqModule module)
      {
         module = null;
         if (this.Handle == IntPtr.Zero)
         {
            return ErrorCode.ErrorHandleNotValid;
         }

         ErrorCode ret = ErrorCode.Success;
         lock (m_cachedModules)
         {
            if (m_cachedModules.TryGetValue(GetModuleKey(type, index), out module))
            {
               return ret;
            }
            // create a new instance for the module
            IntPtr moduleHandle;
            ret = BDaqApi.AdxDeviceGetModuleHandle(this.Handle, type, index, out moduleHandle);
            if (ret == ErrorCode.Success)
            {
               switch (type)
               {
                  case ModuleType.DaqAi:
                     module = new BDaqAiModule(moduleHandle);
                     break;
                  case ModuleType.DaqAo:
                     module = new BDaqAoModule(moduleHandle);
                     break;
                  case ModuleType.DaqDio:
                     module = new BDaqDioModule(moduleHandle);
                     break;
                  case ModuleType.DaqCounter:
                     module = new BDaqCntrModule(moduleHandle);
                     break;
               }
               m_cachedModules[GetModuleKey(type, index)] = module;
            }
         }
         return ret;
      }

      public ErrorCode RefreshProperties()
      {
         return BDaqApi.AdxDeviceRefreshProperties(this.Handle);
      }

      public ErrorCode ShowModalDialog(IntPtr parentWnd, int dataSource)
      {
         IntPtr dlgWnd;
         return BDaqApi.AdxDeviceShowConfigDialogBox(this.Handle, parentWnd, IntPtr.Zero, dataSource, 1, out dlgWnd);
      }
      public ErrorCode ShowPopupDialog(IntPtr parentWnd, int dataSource, out IntPtr dlgWnd)
      {
         return BDaqApi.AdxDeviceShowConfigDialogBox(this.Handle, parentWnd, IntPtr.Zero, dataSource, 0, out dlgWnd);
      }
      public ErrorCode ShowEmbedDialog(IntPtr parentWnd, IntPtr wndRect, int dataSource, out IntPtr dlgWnd)
      {
         return BDaqApi.AdxDeviceShowConfigDialogBox(this.Handle, parentWnd, wndRect, dataSource, 0, out dlgWnd);
      }
      public ErrorCode Reset(int state)
      {
         return BDaqApi.AdxDeviceReset(this.Handle, state);
      }

      public void Close()
      {
         Dispose();
      }

      public void Dispose()
      {
         Dispose(true);
      }

      public int DeviceNumber
      {
         get { return m_deviceNumber; }
      }
      public AccessMode Mode
      {
         get { return m_deviceMode; }
      }
      #endregion

      #region methods for internal using only

      // user can only initializes a new instance by the static method 'Open'
      protected BDaqDevice(IntPtr deviceHandle, int deviceNumber, AccessMode mode)
         : base(deviceHandle)
      {
         m_deviceNumber = deviceNumber;
         m_deviceMode = mode;
      }
      ~BDaqDevice()
      {
         Dispose(false);
      }

      private void Dispose(bool explicitDisposing)
      {
         lock (s_cachedDevices)
         {
            // the method is called directly or indirectly from user's code, decrement  
            // the reference count of unmanaged resource.
            if (explicitDisposing)
            {
               --m_refCount;
            }
            else
            {
               // the method is called by runtime, forcedly reset the reference count to zero
               m_refCount = 0;
            }

            // Check to see if Dispose has already been called.
            if (!m_disposed && m_refCount <= 0)
            {
               BDaqApi.AdxDeviceClose(this.Handle);
               s_cachedDevices.Remove(GetDeviceKey(m_deviceNumber, m_deviceMode));
               this.Handle = IntPtr.Zero;
               m_disposed = true;
            }
         }
      }
      private static int GetModuleKey(ModuleType type, int index)
      {
         return (int)type | (index << 16);
      }
      private static int GetDeviceKey(int deviceNumber, AccessMode mode)
      {
         if (mode == AccessMode.ModeWriteWithReset)
         {
            mode = AccessMode.ModeWrite;
         }
         return (int)deviceNumber | ((int)mode << 16);
      }
      #endregion

      #region fields
      protected int m_deviceNumber = -1;
      protected AccessMode m_deviceMode = AccessMode.ModeRead;
      private int m_refCount = 0;
      private bool m_disposed = false;
      private readonly Dictionary<int, BDaqModule> m_cachedModules = new Dictionary<int, BDaqModule>();

      // device map, used to implement the 'singleton' for the devices which own the same device number and access mode. 
      private static readonly Dictionary<int, BDaqDevice> s_cachedDevices = new Dictionary<int, BDaqDevice>();
      #endregion

      #region private types
      private class BDaqAiModule : BDaqAi
      {
         public BDaqAiModule(IntPtr aiHandle) : base(aiHandle) { }
      }
      private class BDaqAoModule : BDaqAo
      {
         public BDaqAoModule(IntPtr aoHandle) : base(aoHandle) { }
      }
      private class BDaqDioModule : BDaqDio
      {
         public BDaqDioModule(IntPtr dioHandle) : base(dioHandle) { }
      }
      private class BDaqCntrModule : BDaqCntr
      {
         public BDaqCntrModule(IntPtr cntrHandle) : base(cntrHandle) { }
      }
      #endregion
   }

   public static class BDaqEnumerator
   {
      public struct EnumDeviceArgs 
      {
         // current device information
         public int DeviceNumber;
         public string Description;
         public BDaqDevice Device; // Note: the device will be closed after the delegate returned.
      } 
      
      public struct EnumModuleArgs
      {
         // parent device information
         public int DeviceNumber;
         public string Description;
         public BDaqDevice Device; // Note: the device WILL BE closed after the delegate returned.

         // current module information
         public int ModuleIndex;
         public BDaqModule Module;
      }

      public delegate bool EnumDeviceDelegate(EnumDeviceArgs args);
      public delegate bool EnumModuleDelegate(EnumModuleArgs args);

      // Enumerate all devices which are on the system
      public static void EnumerateDevices(EnumDeviceDelegate enumDevice)
      {
         EnumerateDevices(ModuleType.DaqAny, enumDevice);
      }

      // Enumerate all devices which are on the system and support the specified function
      public static void EnumerateDevices(ModuleType typeWanted, EnumDeviceDelegate enumDevice)
      {
         int deviceIndex = 0, deviceNumber, subDevCount;
         StringBuilder descBuff = new StringBuilder(256); // device description buffer
         ErrorCode ret;
         bool keepGoing = true;

         // the user doesn't care about the supported function of the device
         if (typeWanted == ModuleType.DaqAny)
         {
            typeWanted = ModuleType.DaqDevice;
         }

         while(keepGoing)
         {
            ret = BDaqApi.AdxDeviceGetLinkageInfo(-1, deviceIndex++, out deviceNumber, descBuff, out subDevCount);
            if (deviceNumber == -1)
            {
               break; // no more device
            }
            if (ret != ErrorCode.Success)
            {
               continue; // device does not be on the system 
            }

            // open the device to do further check
            BDaqDevice device;
            if (BDaqDevice.Open(deviceNumber, AccessMode.ModeRead, out device) == ErrorCode.Success)
            {
               // Does the device support the function the user wanted?
               BDaqModule module;
               if (typeWanted == ModuleType.DaqDevice
                  || device.GetModule(typeWanted, 0, out module) == ErrorCode.Success)
               {
                  // Call the passed-in delegate to let the user do further action
                  EnumDeviceArgs args;
                  args.DeviceNumber = deviceNumber;
                  args.Description = descBuff.ToString();
                  args.Device = device;
                  keepGoing = enumDevice(args);
               }
               // close the device
               device.Close();
            }
         } 
      }

      // Enumerate all modules which are on the system
      public static void EnumerateModules(ModuleType typeWanted, EnumModuleDelegate enumModule)
      {
         EnumerateDevices(
            delegate(EnumDeviceArgs devArgs)
            {
               // save parent device information into our enumeration args
               EnumModuleArgs modArgs;
               modArgs.DeviceNumber = devArgs.DeviceNumber;
               modArgs.Description = devArgs.Description;
               modArgs.Device = devArgs.Device;

               // enumerating all wanted modules
               BDaqModule module;
               for (int i = 0; true; ++i)
               {
                  if (devArgs.Device.GetModule(typeWanted, i, out module) != ErrorCode.Success)
                  {
                     break;
                  }
                  modArgs.ModuleIndex = i;
                  modArgs.Module = module;
                  if (!enumModule(modArgs))
                  {
                     return false; // break the enumeration
                  }
               }
               return true;
            }
         );
      }
   }

#endregion

#region Bionic DAQ Native APIs
   public static class BDaqApi
   {
      #region constants
      public const int DEVICE_DESC_MAX_LEN = 64;
      public const int VALUE_RANGE_DESC_MAX_LEN = 256;
      public const int SIGNAL_DROP_DESC_MAX_LEN = 256;
      #endregion

      #region global helper APIs
      [DllImport("BioDaq.dll", CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxDeviceGetLinkageInfo(int deviceParent, int index, out int deviceNumber, StringBuilder description, out int subDeviceCount);

      [DllImport("BioDaq.dll", CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxGetValueRangeInformation(ValueRange type, int bufferSize, StringBuilder description, out MathInterval range, out ValueUnit unit);

      [DllImport("BioDaq.dll", CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxGetSignalConnectionInformation(SignalDrop sig, int bufferSize, StringBuilder description, out SignalPosition position);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern double AdxTranslateTemperatureScale(TemperatureDegree degreeType, double degreeCelsius);
      #endregion

      #region event APIs
      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxEventGetHandle(IntPtr module, EventId id, out IntPtr eventHandle);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public unsafe static extern ErrorCode AdxEventGetLastStatus(IntPtr module, EventId id, int* statusLParam, int* statusRParam);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxEventClearFlag(IntPtr module, EventId id, int flagLParam, int flagRParam);
      #endregion

      #region property APIs
      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public unsafe static extern ErrorCode AdxPropertyRead(IntPtr module, PropertyId id, int bufferSize, void* buffer, int* dataLength, int* attribute);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public unsafe static extern ErrorCode AdxPropertyWrite(IntPtr module, PropertyId id, int dataLength, void* buffer, int notifyNow);
      #endregion

      #region device APIs
      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxDeviceOpen(int number, AccessMode accessMode, out IntPtr deviceHandle);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxDeviceClose(IntPtr deviceHandle);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxDeviceReset(IntPtr deviceHandle, int state);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxDeviceGetModuleHandle(IntPtr deviceHandle, ModuleType moduleType, int index, out IntPtr moduleHandle);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxDeviceRefreshProperties(IntPtr deviceHandle);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxDeviceShowConfigDialogBox(IntPtr deviceHandle, IntPtr parentWindow, IntPtr wndRect, int dataSource, int modal, out IntPtr dialogWindow);

      #endregion

      #region Analog Input APIs
      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public unsafe static extern ErrorCode AdxAiReadSamples(IntPtr aiHandle, int chStart, int chCount, void* dataRaw, double* dataScaled);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedAiPrepare(IntPtr aiHandle, int dataCount, out IntPtr dataRaw);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedAiRunOnce(IntPtr aiHandle, int asynchronous);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedAiRun(IntPtr aiHandle);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedAiStop(IntPtr aiHandle);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedAiRelease(IntPtr aiHandle);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public unsafe static extern ErrorCode AdxBufferedAiScaleData(IntPtr aiHandle, void* dataRaw, double* dataScaled, int dataCount, ref int chOffset);
      #endregion

      #region Analog Output APIs
      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public unsafe static extern ErrorCode AdxAoWriteSamples(IntPtr aoHandle, int chStart, int chCount, void* dataRaw, double* dataScaled);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedAoPrepare(IntPtr aoHandle, int dataCount, out IntPtr dataRaw);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedAoRunOnce(IntPtr aoHandle, int asynchronous);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedAoRun(IntPtr aoHandle);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedAoStop(IntPtr aoHandle, int action);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedAoRelease(IntPtr aoHandle);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public unsafe static extern ErrorCode AdxBufferedAoScaleData(IntPtr aoHandle, double* dataScaled, void* dataRaw, int dataCount, ref int channelOffset);
      #endregion

      #region Digital Input/Output APIs
      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public unsafe static extern ErrorCode AdxDiReadPorts(IntPtr dioHandle, int portStart, int portCount, byte* buffer);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public unsafe static extern ErrorCode AdxDoWritePorts(IntPtr dioHandle, int portStart, int portCount, byte* buffer);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public unsafe static extern ErrorCode AdxDoReadBackPorts(IntPtr dioHandle, int portStart, int portCount, byte* buffer);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedDiPrepare(IntPtr dioHandle, int sampleCount, out IntPtr dataBuffer);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedDiRunOnce(IntPtr dioHandle, int asynchronous);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedDiRun(IntPtr dioHandle);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedDiStop(IntPtr dioHandle);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedDiRelease(IntPtr dioHandle);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedDoPrepare(IntPtr dioHandle, int sampleCount, out IntPtr dataBuffer);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedDoRunOnce(IntPtr dioHandle, int asynchronous);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedDoRun(IntPtr dioHandle);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedDoStop(IntPtr dioHandle, int action);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxBufferedDoRelease(IntPtr dioHandle);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxDiSnapStart(IntPtr dioHandle, EventId id, int portStart, int portCount, out IntPtr buffer);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxDiSnapStop(IntPtr dioHandle, EventId id);
      #endregion

      #region Counter APIs
      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxCounterReset(IntPtr cntrHandle, int cntrStart, int cntrCount);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxEventCountStart(IntPtr cntrHandle, int cntrStart, int cntrCount);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public unsafe static extern ErrorCode AdxEventCountRead(IntPtr cntrHandle, int cntrStart, int cntrCount, int* eventCount);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public unsafe static extern ErrorCode AdxOneShotStart(IntPtr cntrHandle, int cntrStart, int cntrCount);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public unsafe static extern ErrorCode AdxTimerPulseStart(IntPtr cntrHandle, int cntrStart, int cntrCount);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxFrequencyMeasureStart(IntPtr cntrHandle, int cntrStart, int cntrCount);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public unsafe static extern ErrorCode AdxFrequencyMeasureRead(IntPtr cntrHandle, int cntrStart, int cntrCount, double* frequency);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxPwmInStart(IntPtr cntrHandle, int cntrStart, int groupCount);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public unsafe static extern ErrorCode AdxPwmInRead(IntPtr cntrHandle, int cntrStart, int groupCount, double* hiPeriod, double* lowPeriod);

      [DllImport("BioDaq.dll", CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
      public static extern ErrorCode AdxPwmOutStart(IntPtr cntrHandle, int cntrStart, int cntrCount);

      #endregion
   }
#endregion
}

