using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Windows.Forms;
using T4;
using T4.API;
using T4.Connections;
using T4.Messages;
using T4.TraceListener;
using static T4.APIPlus.AccountList.PositionUpdateList;







namespace OrderPanel
{



    public partial class OrderPanel : Form
    {
        //member variables
        internal Host moHost;
        internal Market moMarket;
        internal Exchange moExchange;
        internal Contract moContract;
        internal Order moOrder;
        internal MarketDepth moDepth;
        internal Account moAccounts;

        //initialize thr app using startup code
        private void Init()
        {
            Trace.WriteLine("Init");

            

            // Populate the available exchanges.
            DisplayExchanges();
            

            

        }

        #region marketpicker
        //display exchanges
        private void DisplayExchanges()
        {
            // First clear all the combo's.
            ExchangePickerBox.Items.Clear();
            ContractPickerBox.Items.Clear();
            MarketPickerBox.Items.Clear();

            // Eliminate any previous references.
            moExchange = null;
            moContract = null;
            moMarket = null;

            // Populate the list of exchanges.
            try
            {
                // Add the exchanges to the dropdown list.
                foreach (Exchange oExchange in moHost.MarketData.Exchanges.GetSortedList())
                {
                    ExchangePickerBox.Items.Add(oExchange);
                }


            }
            catch (Exception ex)
            {
                // Trace the error.
                Trace.WriteLine("Error " + ex.ToString());
            }
        }
        private void ExchangePickerBox_SelectedIndexChanged(Object sender, System.EventArgs e)
        {

            // Populate the current exchange's available contracts.
            if (ExchangePickerBox.SelectedItem != null)
            {

                // Reference the current exchange.
                moExchange = ((Exchange)(ExchangePickerBox.SelectedItem));

                // Display the contracts
                DisplayContracts();
            }
        }

        private void DisplayContracts()
        {

            // First clear all the combo's.
            ContractPickerBox.Items.Clear();
            MarketPickerBox.Items.Clear();

            // Eliminate any previous references.
            moContract = null;
            moMarket = null;


            if (moExchange != null)
            {

                try
                {
                    // Add the exchanges to the dropdown list.
                    foreach (Contract moContract in moExchange.Contracts)
                    {
                        ContractPickerBox.Items.Add(moContract);
                    }


                }
                catch (Exception ex)
                {
                    // Trace the error.
                    Trace.WriteLine("Error " + ex.ToString());
                }
            }
        }
        private void ContractPickerBox_SelectedIndexChanged(Object sender, System.EventArgs e)
        {

            // Populate the current contract's available markets.

            {

                if ((ContractPickerBox.SelectedItem != null))
                {
                    // Reference the current contract.
                    moContract = (Contract)ContractPickerBox.SelectedItem;

                    // This will return outrights only.
                    moContract.GetMarkets(0, StrategyType.None, e2 =>
                    {
                        if (this.InvokeRequired)
                            this.BeginInvoke(new OnMarketListComplete(DisplayMarkets), new Object[] { e2 });
                        else
                            DisplayMarkets(e2);
                    });

                }
            }
        }

        private void DisplayMarkets(MarketListEventArgs e)
        {
            // Populate the list of markets available for the current contract.

            // First clear the combo.
            MarketPickerBox.Items.Clear();

            // Eliminate any previous references.
            moMarket = null;

            try
            {
                // Add the markets to the dropdown list.
                foreach (Market oMarket in e.Markets.GetSortedList())
                {
                    MarketPickerBox.Items.Add(moMarket);

                }
            }
            catch (Exception ex)
            {
                // Trace the error.
                Trace.WriteLine("Error " + ex.ToString());
            }
        }

        private void MarketPickerBox_SelectedIndexChanged(Object sender, System.EventArgs e)
        {
            if (MarketPickerBox.SelectedItem != null)
            {
                // Store a reference to the current market.
                moMarket = ((Market)(MarketPickerBox.SelectedItem));
            }
        }

        #endregion


        












        #region api and form startup init ref
        //Initialization API startup code
        public OrderPanel()
        {

            
            InitializeComponent();
            moHost = Host.Login(APIServerType.Simulator, "T4Example", "112A04B0-5AAF-42F4-994E-FA7CB959C60B");

            // Check for success.
            if (moHost == null)
            {
                // Host object not returned which means the user cancelled the login dialog.
                this.Close();
            }
            else
            {
                // Login was successfull.
                Trace.WriteLine("Login Success");

                //Initialize
                Init();
            }


           

        }
        
        #endregion
        

        #region Keypad CTS IGNORE
        //LOTSIZE KEYPAD//1LOT
        private void button1_Click_1(object sender, EventArgs e)
        {
            //increase by one
            numericUpDown1.Value += 1;
        }
        //5LOT
        private void button2_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value += 5;
        }
        //10LOT
        private void button3_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value += 10;
        }
        //50LOT
        private void button4_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value += 50;
        }
        //100 LOT
        private void button5_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value += 100;
        }
        //clear button
        private void button6_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = 0;
        }
        //keypad maxmin
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.Minimum = 0;
            numericUpDown1.Maximum = 100000000;
        }


















        #endregion

       
    }

    



}

