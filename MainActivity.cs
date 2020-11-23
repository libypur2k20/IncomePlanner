using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

namespace IncomePlanner
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        EditText incomePerHourEditText;
        EditText workHourPerDayEditText;
        EditText taxRateEditText;
        EditText savingRateEditText;

        TextView annualWorkSummaryTextView;
        TextView annualGrossIncomeTextView;
        TextView annualTaxPayableTextView;
        TextView annualSavingsTextView;
        TextView spendableIncomeTextView;

        Button calculateButton;
        RelativeLayout resultLayout;

        bool inputCalculated = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            ConnectViews();

        }

        private void CalculateButton_Click(object sender, System.EventArgs e)
        {
            if (inputCalculated == false)
            {
                //Annual Work Summary
                double totalHours = double.Parse(workHourPerDayEditText.Text) * (5 * 50); //5 days of work per week. 50 working weeks per year.
                annualWorkSummaryTextView.Text = totalHours.ToString("N0") + " HRS";

                //Annual Gross Income
                double grossIncome = totalHours * double.Parse(incomePerHourEditText.Text);
                annualGrossIncomeTextView.Text = grossIncome.ToString("N0") + " USD";

                //Annual Tax Payable
                double taxPayable = grossIncome * (double.Parse(taxRateEditText.Text) / 100);
                if (taxPayable > grossIncome) taxPayable = grossIncome;
                annualTaxPayableTextView.Text = taxPayable.ToString("N0") + " USD";


                double availableForSavings = (grossIncome - taxPayable);

                //Annual Savings

                double annualSavings = grossIncome * (double.Parse(savingRateEditText.Text) / 100);
                annualSavings = (availableForSavings < annualSavings ? availableForSavings : annualSavings);
                annualSavingsTextView.Text = annualSavings.ToString("N0") + " USD";

                spendableIncomeTextView.Text = (availableForSavings - annualSavings).ToString("N0") + " USD";

            }

            ToggleState();


        }


        void ToggleState()
        {
            if (inputCalculated == true)
                ClearInputs();

            inputCalculated = !inputCalculated;

            ToggleResultsVisibility();

            calculateButton.Text = (inputCalculated == true ? "Clear" : "Calculate");

        }


        void ClearInputs()
        {
            incomePerHourEditText.Text = "";
            workHourPerDayEditText.Text = "";
            taxRateEditText.Text = "";
            savingRateEditText.Text = "";

            incomePerHourEditText.RequestFocus();
        }

        void ConnectViews()
        {
            incomePerHourEditText = FindViewById<EditText>(Resource.Id.incomePerHourEditText);
            workHourPerDayEditText = FindViewById<EditText>(Resource.Id.workHoursEditText);
            taxRateEditText = FindViewById<EditText>(Resource.Id.taxRateEditText);
            savingRateEditText = FindViewById<EditText>(Resource.Id.savingsRateEditText);

            annualWorkSummaryTextView = FindViewById<TextView>(Resource.Id.annualWorkSummaryTextView);
            annualGrossIncomeTextView = FindViewById<TextView>(Resource.Id.annualGrossIncomeTextView);
            annualTaxPayableTextView = FindViewById<TextView>(Resource.Id.annualTaxPayableTextView);
            annualSavingsTextView = FindViewById<TextView>(Resource.Id.annualSavingsTextView);
            spendableIncomeTextView = FindViewById<TextView>(Resource.Id.spendableIncomeTextView);

            calculateButton = FindViewById<Button>(Resource.Id.calculateButton);
            resultLayout = FindViewById<RelativeLayout>(Resource.Id.resultLayout);

            calculateButton.Click += CalculateButton_Click;
        }

        void ToggleResultsVisibility()
        {
            resultLayout.Visibility = (resultLayout.Visibility == Android.Views.ViewStates.Visible ? Android.Views.ViewStates.Invisible : Android.Views.ViewStates.Visible);
        }

    }
}