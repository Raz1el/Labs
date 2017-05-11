using System;
using System.Collections.Generic;
using System.Windows;
using GaloisFieldLib;
using GaloisFieldLib.Сyclic_codes;

namespace Labs
{
    public partial class BchCodding_Code
    {
        readonly ulong CharOfField; // характеристика
        readonly int CountError  ; // количество ошибок
        readonly ulong[] GeneratingFieldArray; // полином, порождающий поле
        private static BchCode _bch;
        private static Polynomial _msg2; //закодированное сообщение
        private static bool _oprvvoda = false;


        public BchCodding_Code(ulong characteristicofField, int countMistake, ulong[] figure)
            
        {
          

            try
            {
                CharOfField = characteristicofField;
                CountError = countMistake;
                GeneratingFieldArray = figure;
                InitializeComponent();

                var generator = new Polynomial(GeneratingFieldArray);
                _bch = new BchCode(CountError, CharOfField, generator);
                var lengthOfMessage = _bch.CodeLength;
                var numOfInformationSymbols = _bch.NumberOfInformationSymbols;
                var countOfMistake = _bch.ErrorNumber;

                string str1 = Convert.ToString(lengthOfMessage);
                string str2 = Convert.ToString(numOfInformationSymbols);
                string str3 = Convert.ToString(countOfMistake);
                string str4 = ",";
                InfoTextBox.Text = "(" + str1 + str4 + str2 + str4 + str3 + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public BchCodding_Code()
        {
            try
            {
                InitializeComponent();

                var generator = new Polynomial(GeneratingFieldArray);
                _bch = new BchCode(CountError, CharOfField, generator);
                var lengthOfMessage = _bch.CodeLength;
                var numOfInformationSymbols = _bch.NumberOfInformationSymbols;
                var countOfMistake = _bch.ErrorNumber;

                string str1 = Convert.ToString(lengthOfMessage);
                string str2 = Convert.ToString(numOfInformationSymbols);
                string str3 = Convert.ToString(countOfMistake);
                string str4 = ",";
                InfoTextBox.Text = "(" + str1 + str4 + str2 + str4 + str3 + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //ввод сообщения и вывод кодируемого сообщения
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ввод полинома
                ulong[] figureArray;
                if (PolinomtextBox.Text != string.Empty)
                {
                    var list = new List<ulong>();
                    char[] delim = {' ', ','};

                    string entervalue = Convert.ToString(PolinomtextBox.Text);
                    string[] arrayofnumber = entervalue.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var w in arrayofnumber)
                    {
                        list.Add(Convert.ToUInt64(w));
                    }
                    figureArray = list.ToArray();
                    _oprvvoda = true;
                }
                else
                {
                    MessageBox.Show("Введите полином сообщения!");
                    ClearMethod();
                    _oprvvoda = false;
                    return;
                }

                if (figureArray.Length > (int) _bch.NumberOfInformationSymbols)
                {
                    MessageBox.Show("Длина входного сообщения не должна быть больше числа информационных символов!");
                    ClearMethod();
                    return;
                }

                var msg1 = new Polynomial(figureArray);
                _msg2 = _bch.Code(msg1); //закодировали сообщение
                CodeTextBox.Text = Convert.ToString(_msg2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //ввод ошибки
        private void EnterErrorButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_oprvvoda == false)
                {
                    throw new Exception("Сначала введите кодируемое сообщение!");
                }
                //ввод полинома
                ulong[] figureArray;
                if (ErrorTextBox.Text != string.Empty)
                {
                    var list = new List<ulong>();
                    char[] delim = { ' ', ',' };

                    string entervalue = Convert.ToString(ErrorTextBox.Text);
                    string[] arrayofnumber = entervalue.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var w in arrayofnumber)
                    {
                        list.Add(Convert.ToUInt64(w));
                    }
                    figureArray = list.ToArray();
                }
                else
                {
                    MessageBox.Show("Введите полином ошибки!");
                    ClearMethod();
                    return;
                }

                var error = new Polynomial(figureArray);
                var msg3 = error + _msg2; //закодированное сообщение с ошибкой
                msg3.SetMod(CharOfField);
                msg3.Reduce();
                MessWithErrorTextBox.Text = Convert.ToString(msg3);

             

                var msg4 = _bch.Decode(msg3);
                RecoveryTextBox.Text = Convert.ToString(msg4);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //очистить поля
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearMethod();
        }

        //выход
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //метод очистки
        private void ClearMethod()
        {
            PolinomtextBox.Clear();
            CodeTextBox.Clear();
            ErrorTextBox.Clear();
            MessWithErrorTextBox.Clear();
            RecoveryTextBox.Clear();
        }
    }
}
