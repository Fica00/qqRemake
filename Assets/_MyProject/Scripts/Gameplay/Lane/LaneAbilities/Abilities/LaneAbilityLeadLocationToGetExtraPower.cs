using UnityEngine;

public class LaneAbilityLeadLocationToGetExtraPower : LaneAbilityBase
{
    [SerializeField] private int addPower;

    private bool[] appliedPower = new bool[2];//0 for player, 1 for opponent

    public override void Subscribe()
    {
        for (int i = 0; i < appliedPower.Length; i++)
        {
            appliedPower[i] = false;
        }

        TableHandler.OnRevealdCard += CheckPower;
        CalculatePower();
    }

    private void OnDisable()
    {
        TableHandler.OnRevealdCard -= CheckPower;
    }

    private void CheckPower(CardObject _cardObject)
    {
        if (_cardObject.LaneLocation != laneDisplay.Location)
        {
            return;
        }

        CalculatePower();
    }

    private void CalculatePower()
    {
        int _myCalculatedPower = GameplayManager.Instance.TableHandler.GetPower(true, laneDisplay.Location);
        int _opponentCalculatedPower = GameplayManager.Instance.TableHandler.GetPower(false, laneDisplay.Location);

        TryToRevard(_myCalculatedPower, _opponentCalculatedPower, 0);
        TryToRevard(_opponentCalculatedPower, _myCalculatedPower, 1);

        void TryToRevard(int _myPower, int _opponentPower, int _playerNumber)
        {
            if (_myPower > _opponentPower)
            {
                if (!appliedPower[_playerNumber])
                {
                    appliedPower[_playerNumber] = true;
                    ChangePower(_playerNumber, addPower);
                }
            }
            else
            {
                if (appliedPower[_playerNumber])
                {
                    appliedPower[_playerNumber] = false;
                    ChangePower(_playerNumber, -addPower);
                }
            }
        }

        void ChangePower(int _playerNumber, int _amount)
        {
            LaneDisplay _lane1 = null;
            LaneDisplay _lane2 = null;

            switch (laneDisplay.Location)
            {
                case LaneLocation.Top:
                    _lane1 = GameplayManager.Instance.Lanes[1];
                    _lane2 = GameplayManager.Instance.Lanes[2];
                    break;
                case LaneLocation.Mid:
                    _lane1 = GameplayManager.Instance.Lanes[0];
                    _lane2 = GameplayManager.Instance.Lanes[2];
                    break;
                case LaneLocation.Bot:
                    _lane1 = GameplayManager.Instance.Lanes[0];
                    _lane2 = GameplayManager.Instance.Lanes[1];
                    break;
            }

            _lane1.LaneSpecifics.ChangeExtraPower(_playerNumber, _amount);
            _lane2.LaneSpecifics.ChangeExtraPower(_playerNumber, _amount);
        }
    }
}
