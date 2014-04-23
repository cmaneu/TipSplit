using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TipSplit.ViewModel
{
    internal class SplitOperationViewModel : BaseViewModel
    {
        private double _totalAmount;
        private double _totalAmountWithTips;
        private int _numberOfParts;
        private int _tipsPercentage;
        private double _amountPerPart;
        private double _tipAmountPerPart;
        private ICommand _changeTipAmountCommand;

        public ICommand ChangeTipAmountCommand  
        {
            get { return _changeTipAmountCommand; }
            set
            {
                if (Equals(value, _changeTipAmountCommand)) return;
                _changeTipAmountCommand = value;
                RaisePropertyChanged();
            }
        }

        public double TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                SetProperty(ref _totalAmount, value);
                RaisePropertyChanged("TotalAmountDisplay");
                ComputePart();
            }
        }  
        
        public double TotalAmountWithTips
        {
            get { return _totalAmountWithTips; }
            set { SetProperty(ref _totalAmountWithTips, value); }
        }

        public string TotalAmountDisplay
        {
            get
            {
                // TODO: Format according to locale
                return _totalAmount.ToString();
            }
        }

        public int NumberOfParts
        {
            get { return _numberOfParts; }
            set
            {
                if (value <= 0)
                {
                    value = 1;
                }

                SetProperty(ref _numberOfParts, value);
                ComputePart();
            }
        }

        public int TipsPercentage
        {
            get { return _tipsPercentage; }
            set
            {
                SetProperty(ref _tipsPercentage, value);
                ComputePart();
            }
        }

        public double AmountPerPart
        {
            get { return _amountPerPart; }
            set
            {
                SetProperty(ref _amountPerPart, value);
                RaisePropertyChanged("AmountPerPartDisplay");
            }
        }


        public string AmountPerPartDisplay
        {
            get
            {
                var userCurrency = Windows.System.UserProfile.GlobalizationPreferences.Currencies[0];
                // Nombre à afficher. 

                // Création d’un formatter en utilisant les préférences utilisateur actuelles.
                var userCurrencyFormat = new Windows.Globalization.NumberFormatting.CurrencyFormatter(userCurrency);
                return userCurrencyFormat.Format(Math.Round(_amountPerPart, 2)); 
            }
           
        }

        public double TipAmountPerPart
        {
            get { return _tipAmountPerPart; }
            set { SetProperty(ref _tipAmountPerPart, value); }
        }


        public SplitOperationViewModel()
        {
            NumberOfParts = 1;
            TipsPercentage = 0;
            ChangeTipAmountCommand = new DelegateCommand<int>(ExecuteChangeTipAmount);
        }

        private void ExecuteChangeTipAmount(int i)
        {
            TipsPercentage = i;
        }

        public void ComputePart()
        {
            if (TotalAmount == 0)
            {
                TipAmountPerPart = 0;
                AmountPerPart = 0;
                return;
            }

            double totalTip = TipsPercentage * 100 / TotalAmount;

            TotalAmountWithTips = TotalAmount + totalTip;
            TipAmountPerPart = totalTip / NumberOfParts;
            AmountPerPart = TotalAmountWithTips / NumberOfParts;

        }
    }
}
