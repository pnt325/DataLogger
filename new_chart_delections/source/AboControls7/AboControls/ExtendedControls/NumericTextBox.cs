using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AboControls.ExtendedControls
{
    /// <summary>
    /// If only one decimal place is to be allowed for input, then I recommend
    /// switching to a numeric up down.
    /// </summary>
    class NumericTextBox : TextBox
    {
        private int _minNumber;
        private int _maxNumber = 9;
        private bool _allowControl = true;
        private bool _allowDecimal = true;
        private bool _pendingFilter;
        private bool _allowNegation = true;
        private bool _allowMultipleDecimals = true;

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

﻿            bool isDecimal = (e.KeyChar.Equals('.'));
             bool isControl = (char.IsControl(e.KeyChar));

             // We are offsetting from this key to get the appropriate number key designations
             const int key0Index = (int)Keys.D0;
             bool withinNumRange = (e.KeyChar >= key0Index + _minNumber &&
                                    e.KeyChar <= key0Index + _maxNumber);
             bool isMinus = (e.KeyChar.Equals('-'));

             if (isDecimal)
             {
                 if (!_allowDecimal)
                 {
                     e.Handled = true;
                 }
                 else if (!_allowMultipleDecimals && DecimalsPresent || DecimalsNearSelection)
                 {
                     e.Handled = true;
                 }
             }

            if (!_allowControl && isControl)
                e.Handled = true;

            if (!isControl && !isDecimal && !isMinus && !withinNumRange)
                e.Handled = true;

            // Keychar 22 is paste I dont know how culture friendly this is
            if (((int)e.KeyChar).Equals(22))
                _pendingFilter = true;

            // If not allowing negative numbers then remove all minus chars
             // otherwise remove all except the one at the start
             if (!_allowNegation && isMinus)
             {
                 e.Handled = true;
             }
             else
             {
                 bool hasMinus = (this.TextLength > 0 && this.Text[0].Equals('-'));

                 if (isMinus)
                 {
                     if (hasMinus || !this.SelectionStart.Equals(0))
                         e.Handled = true;
                 }
             }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            // Remove decimals and or zeros at the end of the text
            const string PATTERN = @"\.(|0+)$";
            this.Text = Regex.Replace(this.Text, PATTERN, string.Empty);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            // Filtering needs to be done after the text is submitted to the textbox.Text property
            // so we must do it here
            if (_pendingFilter)
            {
                FilterText();
            }
        }

        /// <summary>
        /// Filters the text following the same rules applied in the keypress event
        /// </summary>
        private void FilterText()
        {
            for (int i = 0; i < this.Text.Length; i++)
            {
﻿                 bool isDecimal = (this.Text[i].Equals('.'));
                  bool isControl = (char.IsControl(this.Text[i]));
                  const int key0Index = (int)Keys.D0;
                  bool withinNumRange = (this.Text[i] >= key0Index + _minNumber &&
                                          this.Text[i] <= key0Index + _maxNumber);
                  bool isMinus = (this.Text[i].Equals('-'));

                  // First check the assosiated field to see if we should test character for valididty

                  if (!_allowDecimal && isDecimal)
                  {
                      this.Text = this.Text.Remove(i, 1);
                  }

                  if (!_allowControl && isControl)
                  {
                      this.Text = this.Text.Remove(i, 1);
                  }

                  if (!isControl && !isDecimal && !isMinus && !withinNumRange)
                  {
                      this.Text = this.Text.Remove(i, 1);
                  }

                  // If not allowing negative numbers then remove all minus chars
                  // otherwise remove all except the one at the start
                  if (!_allowNegation)
                  {
                      if (isMinus)
                      {
                          this.Text = this.Text.Remove(i, 1);
                      }
                  }
                  else
                  {
                      if (isMinus && i != 0)
                      {
                          this.Text = this.Text.Remove(i, 1);
                      }
                  }
            }

            // If allow decimal then cleanup decimals side by side to follow the same rules
            // applied to the keypress event
            if (_allowDecimal)
            {
                this.Text = Regex.Replace(this.Text, @"[\.]+", @".");
            }

            if (!_allowMultipleDecimals)
            {
                RemoveLeftMostDecimals();
            }

            _pendingFilter = false;
        }

        private void FilterNumbersToSuitRange()
        {
            // Remove numbers below minimum
            for (int i = 0; i < _minNumber; i++)
                this.Text = this.Text.Replace(i.ToString(), string.Empty);

            // Remove numbers above maximum
            for (int i = _maxNumber + 1; i <= 9; i++)
                this.Text = this.Text.Replace(i.ToString(), string.Empty);
        }

        /// <summary>
        /// Removes all decimals except for one on the right if present
        /// </summary>
        private void RemoveLeftMostDecimals()
        {
            bool foundOne = false;

            for (int i = this.TextLength - 1; i >= 0; i--)
            {
                if (this.Text[i].Equals('.'))
                {
                    if (!foundOne)
                        foundOne = true;
                    else
                        this.Text = this.Text.Remove(i, 1);
                }
            }
        }

        #region Properties
        private bool DecimalsNearSelection
        {
            get
            {
                int leftIndex = this.SelectionStart;
                int rightIndex = this.SelectionStart + 1;

                if (this.TextLength > 0 && this.Text[0].Equals('.') && this.SelectionStart == 1)
                {
                    return true;
                }

                if (leftIndex >= 0 && leftIndex < this.TextLength && this.Text[leftIndex].Equals('.'))
                {
                    return true;
                }

                if (rightIndex < this.TextLength - 1 && this.Text[rightIndex].Equals('.'))
                {
                    return true;
                }

                if (this.SelectionStart - 1 > 0 && this.Text[this.SelectionStart - 1].Equals('.'))
                {
                    return true;
                }

                return false;
            }
        }

        private bool DecimalsPresent
        {
            get
            {
                int decimalCount = 0;

                for (int i = 0; i < this.TextLength; i++)
                {
                    if (this.Text[i] == '.')
                    {
                        decimalCount++;
                    }
                }

                return decimalCount > 0;
            }
        }

        [Description("Determines the minimum number of the number key that can be used for input")]
        [DefaultValue(0)]
        [Category("Input")]
        public int MinimumNumber
        {
            get { return _minNumber; }
            set
            {
                // Check bounds
                if (value < 0)
                {
                    _minNumber = 0;
                }
                else if (value > 9)
                {
                    _minNumber = 9;
                }
                else if (value >= _maxNumber)
                {
                    _minNumber = _maxNumber;
                }
                else
                {
                    _minNumber = value;
                }

                FilterNumbersToSuitRange();
            }
        }

        [Description("Determines the maximum number of the number key that can be used for input")]
        [DefaultValue(9)]
        [Category("Input")]
        public int MaximumNumber
        {
            get { return _maxNumber; }
            set
            {
                if (value < 0)
                {
                    _maxNumber = 0;
                }
                else if (value > 9)
                {
                    _maxNumber = 9;
                }
                else if (value <= _minNumber)
                {
                    _maxNumber = _minNumber;
                }
                else
                {
                    _maxNumber = value;
                }

                FilterNumbersToSuitRange();
            }
        }

        [Category("Data")]
        public int IntegerValue
        {
            get
            {
                decimal temp;
                decimal.TryParse(this.Text, out temp);
                return (int)(temp + 0.5m);
            }
            set
            {
                this.Text = value.ToString();
            }
        }

        [Category("Data")]
        public decimal DecimalValue
        {
            get
            {
                decimal result;
                decimal.TryParse(this.Text, out result);
                return result;
            }
            set
            {
                this.Text = value.ToString();
            }
        }

        /// <summary>
        /// Gets whether or not the textbox already has decimals inputted
        /// </summary>
        private bool HasDecimals
        {
            get
            {
                int periodCount = 0;

                for (int i = 0; i < this.Text.Length; i++)
                {
                    if (this.Text[i].Equals('.'))
                    {
                        periodCount++;
                    }
                }

                return (periodCount <= 1);
            }
        }

        [Description("Determines whether decimals can be inputted. More than one decimal is allowed")]
        [Category("Input")]
        public bool AllowDecimal
        {
            get { return _allowDecimal; }
            set
            {
                _allowDecimal = value;
                FilterText();
            }
        }

        [Description("Determines whether control input is allowed, like pasting")]
        [Category("Input")]
        public bool AllowControl
        {
            get { return _allowControl; }
            set
            {
                _allowControl = value;
                FilterText();
            }
        }

        [Description("Determines whether negative numbers can be inputted")]
        [Category("Input")]
        public bool AllowNegation
        {
            get { return _allowNegation; }
            set
            {
                _allowNegation = value;
                FilterText();
            }
        }

        [Description("Determines whether more than one decimal can be inputted")]
        [Category("Input")]
        public bool AllowMultipleDecimals
        {
            get { return _allowMultipleDecimals; }
            set
            {
                _allowMultipleDecimals = value;
                FilterText();
            }
        }
        #endregion
    }
}
