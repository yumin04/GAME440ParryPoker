// 여기서 Camera Group들 나눠야 할듯

namespace Trailer {
	public enum HookCameraPosition {
		PlayerStareAtEachOther,
		P1CloseUpGrabPose,
		P2CloseUpShootPose,
		P2ShootCard,
		FollowCard,
		P1CatchCard,
		P1ShowHand,
		P2ShowHand,
		Pose,
	}

	public enum IntroCameraPosition {
		VsScreenPopUp,
		HealthBarPopUp,
		CardsPopUp,
		Memorize,
	}

	public enum P1GrabThenAttackCameraPosition {
		InitializeCard,
		P1Grab,
		P1CheckCard,
		P1OptionSelection,
		P1ChoosingAttack,
	}

	public enum P2GrabThenAttackCameraPosition {
		InitializeCard,
		P2Grab,
		P2CheckCard,
		P2OptionSelection,
		P2ChoosingAttack,
	}

	public enum P1AttackCameraPosition {
		P1SlotMachine,
		P1SlotMachineOptions,
		P1AimP2AndShoot,
		P2HitByAttack,
		P2HealthDecrease,
	}

	public enum P2AttackCameraPosition {
		P2SlotMachine,
		P2SlotMachineOptions,
		P2AimP2AndShoot,
		P1HitByAttack,
		P1HealthDecrease,
	}

	public enum P1GrabAndKeepCameraPosition {
		InitializeSubRoundCard,
		P1Grab,
		P1CheckCard,
		P1KeepCard,
	}

	public enum P2GrabAndKeepCameraPosition {
		InitializeSubRoundCard,
		P2Grab,
		P2CheckCard,
		P2KeepCard,
	}

	public enum RepeatCameraPosition {
		Repeat14_18_26,
		Repeat19_25_27,
	}

	public enum RisingActionCameraPosition {
		// P2GrabThenAttackCameraPosition
		// P1AttackCameraPosition
		P2QuickSlotMachine,
		P2AimAndShoot,
		P1Hit,
		P1DecreaseHealth
	}

	public enum ClimaxCameraPosition {
		// P2GrabThenAttackCameraPosition but with slow motion
		InitializeCard,
		P2Grab,
		P2CheckCard,
		P2SlowMotionChooseAttack, // Important, duplicate and Use the same position, but add slowmotion
		P2SlotMachine,
		P2AimP1AndShoot,
		P1Hit,
		P1EvilSmile,
		P2LayDownHand,
		P1LayDownHand,
		P1WinPose
	}

	public enum ResultCameraPosition {
		ParryPoker
	}
}
