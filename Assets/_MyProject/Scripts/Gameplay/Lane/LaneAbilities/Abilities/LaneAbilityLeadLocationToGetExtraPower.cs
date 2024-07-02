using System;
using UnityEngine;

public class LaneAbilityLeadLocationToGetExtraPower : LaneAbilityBase
{
    [SerializeField] private int addPower;

    private bool[] appliedPower = new bool[2];//0 for player, 1 for opponent

    public override void Subscribe()
    {
        isSubscribed = true;
        for (int _i = 0; _i < appliedPower.Length; _i++)
        {
            appliedPower[_i] = false;
        }

        TableHandler.OnRevealdCard += CheckPower;
        CalculatePower();
    }

    private void OnDisable()
    {
        if (!isSubscribed)
        {
            return;
        }
        TableHandler.OnRevealdCard -= CheckPower;
    }

    private void CheckPower(CardObject _cardObject)
    {
        GameplayManager.Instance.TableHandler.CalculatePower();
        CalculatePower();
    }

    private void CalculatePower()
    {
        int _myCalculatedPower = GameplayManager.Instance.TableHandler.GetPower(true, laneDisplay.Location);
        int _opponentCalculatedPower = GameplayManager.Instance.TableHandler.GetPower(false, laneDisplay.Location);

        TryToReward(_myCalculatedPower, _opponentCalculatedPower, 0);
        TryToReward(_opponentCalculatedPower, _myCalculatedPower, 1);

        void TryToReward(int _myPower, int _opponentPower, int _playerNumber)
        {
            if (_myPower > _opponentPower)
            {
                if (appliedPower[_playerNumber])
                {
                    return;
                }
                
                appliedPower[_playerNumber] = true;
                ChangePower(_playerNumber, addPower);
            }
            else
            {
                if (!appliedPower[_playerNumber])
                {
                    return;
                }
                appliedPower[_playerNumber] = false;
                ChangePower(_playerNumber, -addPower);
            }
        }

        void ChangePower(int _playerNumber, int _amount)
        {
            LaneDisplay[] _otherLanes = GetOtherLanes(laneDisplay.Location);

            foreach (var _lane in _otherLanes)
            {
                _lane.LaneSpecifics.ChangeExtraPower(_playerNumber, _amount);
            }
        }

        LaneDisplay[] GetOtherLanes(LaneLocation _currentLocation)
        {
            switch (_currentLocation)
            {
                case LaneLocation.Top:
                    return new[] { GameplayManager.Instance.Lanes[1], GameplayManager.Instance.Lanes[2] };
                case LaneLocation.Mid:
                    return new[] { GameplayManager.Instance.Lanes[0], GameplayManager.Instance.Lanes[2] };
                case LaneLocation.Bot:
                    return new[] { GameplayManager.Instance.Lanes[0], GameplayManager.Instance.Lanes[1] };
                default:
                    throw new Exception();
            }
        }
    }
}
