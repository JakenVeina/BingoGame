using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using BingoGame.Commands;
using BingoGame.ViewModels;

namespace BingoGame.Controllers
{
    public class BingoGameController : IBingoGameController
    {
        /**********************************************************************/
        #region Construction

        public BingoGameController()
        {
            GameState = new BingoGameStateViewModel()
            {
                Board = new List<BingoNumberSetViewModel>(),
                History = new ObservableCollection<string>()
            };

            int number = 1;
            foreach (var setName in new[] { "B", "I", "N", "G", "O" })
            {
                var numberSet = new BingoNumberSetViewModel()
                {
                    Numbers = new List<BingoNumberViewModel>()
                };

                foreach (var _ in Enumerable.Repeat(1, 15))
                    numberSet.Numbers.Add(new BingoNumberViewModel()
                    {
                        Name = $"{setName}{number++}",
                        HasBeenCalled = false
                    });

                GameState.Board.Add(numberSet);
            }

            CallNextNumberProvider = new CommandProvider();
            CallNextNumberProvider.CommandExecuted += OnCallNextNumberProviderCommandExecuted;
            CallNextNumberProvider.CommandTested += OnCallNextNumberProviderCommandTested;

            ResetProvider = new CommandProvider();
            ResetProvider.CommandExecuted += OnResetProviderCommandExecuted;
            ResetProvider.CommandTested += OnResetProviderCommandTested;

            PopulateNumberPool();
        }

        #endregion Construction

        /**********************************************************************/
        #region IBingoGameController

        public BingoGameStateViewModel GameState { get; }

        public ICommand CallNextNumber
            => CallNextNumberProvider.Command;
        internal protected ICommandProvider CallNextNumberProvider { get; }

        public ICommand Reset
            => ResetProvider.Command;
        internal protected ICommandProvider ResetProvider { get; }

        #endregion IBingoGameController

        /**********************************************************************/
        #region Private Methods

        private void OnCallNextNumberProviderCommandExecuted(object sender, CommandExecutedEventArgs e)
        {
            var numberIndex = GetNumberFromPool() - 1;
            var numberSetIndex = numberIndex / 15;
            var numberInSetIndex = numberIndex % 15;

            var numberViewModel = GameState.Board[numberSetIndex].Numbers[numberInSetIndex];
            GameState.History.Insert(0, numberViewModel.Name);
            numberViewModel.HasBeenCalled = true;

            if (_numberPool.Count == 74)
                ResetProvider.RaiseCommandCanExecuteChanged();
            else if (_numberPool.Count == 0)
                CallNextNumberProvider.RaiseCommandCanExecuteChanged();
        }

        private void OnCallNextNumberProviderCommandTested(object sender, CommandTestedEventArgs e)
            => e.CanExecute = _numberPool.Count > 0;

        private void OnResetProviderCommandExecuted(object sender, CommandExecutedEventArgs e)
        {
            foreach (var numberSet in GameState.Board)
                foreach (var number in numberSet.Numbers)
                    number.HasBeenCalled = false;

            if (_numberPool.Count == 0)
                CallNextNumberProvider.RaiseCommandCanExecuteChanged();

            GameState.History.Clear();
            _numberPool.Clear();
            PopulateNumberPool();

            ResetProvider.RaiseCommandCanExecuteChanged();
        }

        private void OnResetProviderCommandTested(object sender, CommandTestedEventArgs e)
            => e.CanExecute = _numberPool.Count < 75;

        private int GetNumberFromPool()
        {
            var index = _random.Next(0, (_numberPool.Count));

            var number = _numberPool[index];
            _numberPool.RemoveAt(index);

            return number;
        }

        private void PopulateNumberPool()
        {
            foreach (var number in Enumerable.Range(1, 75))
                _numberPool.Add(number);
        }

        #endregion Private Methods

        /**********************************************************************/
        #region Private Fields

        private Random _random = new Random();

        private List<int> _numberPool = new List<int>(75);

        #endregion Private Fields
    }
}
