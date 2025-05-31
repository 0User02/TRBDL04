/// MainWindow
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }


    private void ButtonPaymentMethods_Click(object sender, RoutedEventArgs e)
    {
        new PaymentMethodsWindow().Show();
        this.Close();
    }

    private void ButtonTax_Click(object sender, RoutedEventArgs e)
    {
        new TaxWindow().Show();
        Close();
    }

    private void ButtonTaxpayers_Click(object sender, RoutedEventArgs e)
    {
        new TaxpayersWindow().Show();
        this.Close();
    }

    private void ButtonPayments_Click(object sender, RoutedEventArgs e)
    {
        new PaymentsWindow().Show();
        Close();
    }

}

/// PaymentMethodsWindow
public partial class PaymentMethodsWindow : Window
{
    public PaymentMethodsWindow()
    {
        InitializeComponent();
        DataGridPaymentMethods.ItemsSource = Учет_платежей_налоговEntities.GetContext().Payment_methods.ToList();
    }

    private void ButtonAdd_Click(object sender, RoutedEventArgs e)
    {
        new AddPaymentMethodWindow().Show();
        Close();
    }

    private void ButtonDelete_Click(object sender, RoutedEventArgs e)
    {
        var method = (sender as Button).DataContext as Payment_methods;

        if (method == null)
            return;

        if (MessageBox.Show($"Удалить метод оплаты \"{method.Name}\"?", "Подтверждение", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;

        try
        {
            using (var context = new Учет_платежей_налоговEntities())
            {
                var attached = context.Payment_methods.Find(method.Method_ID);
                if (attached != null)
                {
                    context.Payment_methods.Remove(attached);
                    context.SaveChanges();
                }
            }

            MessageBox.Show("Удалено.");
        }
        catch (System.Data.Entity.Infrastructure.DbUpdateException)
        {
            MessageBox.Show("Невозможно удалить: данный метод оплаты используется в таблице платежей.",
                            "Ошибка удаления",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }

        // Обновляем таблицу
        DataGridPaymentMethods.ItemsSource = Учет_платежей_налоговEntities.GetContext().Payment_methods.ToList();
    }

    private void ButtonBack_Click(object sender, RoutedEventArgs e)
    {
        new MainWindow().Show();
        this.Close();
    }
    private void ButtonEdit_Click(object sender, RoutedEventArgs e)
    {
        var selected = (Payment_methods)DataGridPaymentMethods.SelectedItem;
        new EditPaymentMethodWindow(selected).Show();
        Close();
    }
}

/// PaymentsWindow
public partial class PaymentsWindow : Window
{
    public PaymentsWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        DataGridPayments.ItemsSource = Учет_платежей_налоговEntities.GetContext().Payments
            .Include(p => p.Tax)
            .Include(p => p.Taxpayers)
            .Include(p => p.Payment_methods)
            .Include(p => p.Tax_period)
            .ToList();
    }

    private void ButtonAdd_Click(object sender, RoutedEventArgs e)
    {
        new AddPaymentWindow().Show();
        Close();
    }

    private void ButtonEdit_Click(object sender, RoutedEventArgs e)
    {
        var payment = (sender as Button).DataContext as Payments;
        new EditPaymentWindow(payment).Show();
        Close();
    }

    private void ButtonDelete_Click(object sender, RoutedEventArgs e)
    {
        var selected = (sender as Button).DataContext as Payments;

        if (selected == null) return;

        if (MessageBox.Show("Удалить выбранный платёж?", "Подтверждение", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;

        try
        {
            var paymentInDb = Учет_платежей_налоговEntities.GetContext().Payments
                .FirstOrDefault(p => p.Payment_Id == selected.Payment_Id);

            if (paymentInDb == null)
            {
                MessageBox.Show("Платёж не найден.");
                return;
            }

            Учет_платежей_налоговEntities.GetContext().Payments.Remove(paymentInDb);
            Учет_платежей_налоговEntities.GetContext().SaveChanges();
            MessageBox.Show("Платёж удалён.");
        }
        catch (System.Data.Entity.Infrastructure.DbUpdateException)
        {
            MessageBox.Show("Невозможно удалить: платёж связан с другими данными.",
                "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        LoadData();
    }

    private void ButtonBack_Click(object sender, RoutedEventArgs e)
    {
        new MainWindow().Show();
        this.Close();
    }
}

/// TaxpayersWindow
public partial class TaxpayersWindow : Window
{
    public TaxpayersWindow()
    {
        InitializeComponent();
        DataGridTaxpayers.ItemsSource = Учет_платежей_налоговEntities.GetContext().Taxpayers.ToList();
    }

    private void ButtonAdd_Click(object sender, RoutedEventArgs e)
    {
        new AddTaxpayerWindow().Show();
        Close();
    }

    private void ButtonEdit_Click(object sender, RoutedEventArgs e)
    {
        var taxpayer = (sender as Button).DataContext as Taxpayers;
        new EditTaxpayerWindow(taxpayer).Show();
        Close();
    }

    private void ButtonDelete_Click(object sender, RoutedEventArgs e)
    {
        var taxpayer = (sender as Button).DataContext as Taxpayers;

        if (taxpayer == null)
            return;

        if (MessageBox.Show($"Удалить налогоплательщика \"{taxpayer.Full_name}\"?", "Подтверждение", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
        try
        {
            Учет_платежей_налоговEntities.GetContext().Taxpayers.Remove(taxpayer);
            Учет_платежей_налоговEntities.GetContext().SaveChanges();
            MessageBox.Show("Удалено.");
        }
        catch (System.Data.Entity.Infrastructure.DbUpdateException)
        {
            MessageBox.Show("Невозможно удалить: данный налогоплательщик используется в таблице платежей.",
                            "Ошибка удаления",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }

        DataGridTaxpayers.ItemsSource = Учет_платежей_налоговEntities.GetContext().Taxpayers.ToList();
    }

    private void ButtonBack_Click(object sender, RoutedEventArgs e)
    {
        new MainWindow().Show();
        this.Close();
    }
}

/// TaxWindow
public partial class TaxWindow : Window
{
    public TaxWindow()
    {
        InitializeComponent();
        DataGridTax.ItemsSource = Учет_платежей_налоговEntities.GetContext().Tax.ToList();
    }

    private void ButtonAdd_Click(object sender, RoutedEventArgs e)
    {
        new AddTaxWindow().Show();
        Close();
    }

    private void ButtonEdit_Click(object sender, RoutedEventArgs e)
    {
        var tax = (sender as Button).DataContext as Tax;
        new EditTaxWindow(tax).Show();
        Close();
    }

    private void ButtonDelete_Click(object sender, RoutedEventArgs e)
    {
        var tax = (sender as Button).DataContext as Tax;

        try
        {
            Учет_платежей_налоговEntities.GetContext().Tax.Remove(tax);
            Учет_платежей_налоговEntities.GetContext().SaveChanges();
            MessageBox.Show("Налог удалён.");
        }
        catch (System.Data.Entity.Infrastructure.DbUpdateException)
        {
            MessageBox.Show("Невозможно удалить: этот налог уже используется в таблице платежей.",
                            "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        DataGridTax.ItemsSource = Учет_платежей_налоговEntities.GetContext().Tax.ToList();
    }

    private void ButtonBack_Click(object sender, RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }
}

/// AddPaymentMethodWindow
public partial class AddPaymentMethodWindow : Window
{
    public AddPaymentMethodWindow()
    {
        InitializeComponent();
    }

    private void ButtonSave_Click(object sender, RoutedEventArgs e)
    {
        string name = NameBox.Text;
        string description = DescriptionBox.Text;

        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show("Поле \"Название\" не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var newMethod = new Payment_methods
            {
                Name = name,
                Description = description
            };

            var context = new Учет_платежей_налоговEntities(); // локальный чистый контекст

            context.Payment_methods.Add(newMethod);
            context.SaveChanges();

            MessageBox.Show("Метод оплаты добавлен!");
            new PaymentMethodsWindow().Show();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка при добавлении:\n" + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


    private void ButtonBack_Click(object sender, RoutedEventArgs e)
    {
        new PaymentMethodsWindow().Show();
        this.Close();
    }
}

/// AddPaymentWindow
public partial class AddPaymentWindow : Window
{
    private Payments _payment = new Payments();

    public AddPaymentWindow()
    {
        InitializeComponent();

        TaxpayerBox.ItemsSource = Учет_платежей_налоговEntities.GetContext().Taxpayers.ToList();
        TaxBox.ItemsSource = Учет_платежей_налоговEntities.GetContext().Tax.ToList();
        PeriodBox.ItemsSource = Учет_платежей_налоговEntities.GetContext().Tax_period.ToList();
        MethodBox.ItemsSource = Учет_платежей_налоговEntities.GetContext().Payment_methods.ToList();
    }

    private void ButtonSave_Click(object sender, RoutedEventArgs e)
    {
        if (TaxpayerBox.SelectedItem == null || TaxBox.SelectedItem == null ||
            PeriodBox.SelectedItem == null || MethodBox.SelectedItem == null ||
            string.IsNullOrWhiteSpace(AmountBox.Text) || string.IsNullOrWhiteSpace(StatusBox.Text))
        {
            MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!decimal.TryParse(AmountBox.Text, out decimal amount))
        {
            MessageBox.Show("Неверный формат суммы.");
            return;
        }

        var newPayment = new Payments
        {
            Taxpayer_ID = ((Taxpayers)TaxpayerBox.SelectedItem).Taxpayer_ID,
            Tax_ID = ((Tax)TaxBox.SelectedItem).Tax_ID,
            Period_ID = ((Tax_period)PeriodBox.SelectedItem).Period_ID,
            Method_ID = ((Payment_methods)MethodBox.SelectedItem).Method_ID,
            Amount = amount,
            Date = DateTime.Now,
            Status = StatusBox.Text,
            Currency = CurrencyBox.Text
        };

        try
        {
            Учет_платежей_налоговEntities.GetContext().Payments.Add(newPayment);
            Учет_платежей_налоговEntities.GetContext().SaveChanges();

            MessageBox.Show("Платёж добавлен!");
            new PaymentsWindow().Show();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка при добавлении платежа:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }



    private void ButtonBack_Click(object sender, RoutedEventArgs e)
    {
        new PaymentsWindow().Show();
        Close();
    }
}

/// AddTaxpayerWindow
public partial class AddTaxpayerWindow : Window
{
    private Taxpayers _taxpayer = new Taxpayers();

    public AddTaxpayerWindow()
    {
        InitializeComponent();
        DataContext = _taxpayer;
    }

    private void ButtonSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_taxpayer.Full_name) ||
            string.IsNullOrWhiteSpace(_taxpayer.Type) ||
            string.IsNullOrWhiteSpace(_taxpayer.TIN) ||
            string.IsNullOrWhiteSpace(_taxpayer.Address) ||
            string.IsNullOrWhiteSpace(_taxpayer.Phone_number))
        {
            MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            using (var context = new Учет_платежей_налоговEntities())
            {
                context.Taxpayers.Add(_taxpayer);
                context.SaveChanges();
            }

            MessageBox.Show("Налогоплательщик добавлен!");
            new TaxpayersWindow().Show();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка при добавлении:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ButtonBack_Click(object sender, RoutedEventArgs e)
    {
        new TaxpayersWindow().Show();
        Close();
    }
}

/// AddTaxWindow
public partial class AddTaxWindow : Window
{

    public AddTaxWindow()
    {
        InitializeComponent();
    }

    private void ButtonSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text) || string.IsNullOrWhiteSpace(RateBox.Text))
        {
            MessageBox.Show("Поля \"Название\" и \"Ставка\" обязательны для заполнения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!decimal.TryParse(RateBox.Text, out decimal rate))
        {
            MessageBox.Show("Неверный формат ставки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var newTax = new Tax
            {
                Name = NameBox.Text,
                Rate = rate,
                Description = DescriptionBox.Text
            };

            using (var context = new Учет_платежей_налоговEntities())
            {
                context.Tax.Add(newTax);
                context.SaveChanges();
            }

            MessageBox.Show("Налог добавлен!");
            new TaxWindow().Show();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка при добавлении:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ButtonBack_Click(object sender, RoutedEventArgs e)
    {
        new TaxWindow().Show();
        Close();
    }
}

/// EditPaymentMethodWindow
public partial class EditPaymentMethodWindow : Window
{
    private Payment_methods _method;
    private Payment_methods _methodCopy;


    public EditPaymentMethodWindow(Payment_methods method)
    {
        InitializeComponent();
        _method = method;
        _methodCopy = new Payment_methods
        {
            Method_ID = method.Method_ID,
            Name = method.Name,
            Description = method.Description
        };
        DataContext = _methodCopy;
    }

    private void ButtonSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_methodCopy.Name))
        {
            MessageBox.Show("Поле \"Название\" не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        _method.Name = _methodCopy.Name;
        _method.Description = _methodCopy.Description;

        Учет_платежей_налоговEntities.GetContext().SaveChanges();

        MessageBox.Show("Изменения сохранены!");
        new PaymentMethodsWindow().Show();
        this.Close();
    }



    private void ButtonBack_Click(object sender, RoutedEventArgs e)
    {
        new PaymentMethodsWindow().Show();
        this.Close();
    }
}

/// EditPaymentWindow
public partial class EditPaymentWindow : Window
{
    private Payments _original;

    public EditPaymentWindow(Payments payment)
    {
        InitializeComponent();

        _original = payment;

        TaxpayerBox.ItemsSource = Учет_платежей_налоговEntities.GetContext().Taxpayers.ToList();
        TaxBox.ItemsSource = Учет_платежей_налоговEntities.GetContext().Tax.ToList();
        PeriodBox.ItemsSource = Учет_платежей_налоговEntities.GetContext().Tax_period.ToList();
        MethodBox.ItemsSource = Учет_платежей_налоговEntities.GetContext().Payment_methods.ToList();

        TaxpayerBox.SelectedItem = TaxpayerBox.Items.Cast<Taxpayers>().FirstOrDefault(t => t.Taxpayer_ID == _original.Taxpayer_ID);
        TaxBox.SelectedItem = TaxBox.Items.Cast<Tax>().FirstOrDefault(t => t.Tax_ID == _original.Tax_ID);
        PeriodBox.SelectedItem = PeriodBox.Items.Cast<Tax_period>().FirstOrDefault(p => p.Period_ID == _original.Period_ID);
        MethodBox.SelectedItem = MethodBox.Items.Cast<Payment_methods>().FirstOrDefault(m => m.Method_ID == _original.Method_ID);

        AmountBox.Text = _original.Amount.ToString();
        StatusBox.Text = _original.Status;
        CurrencyBox.Text = _original.Currency;
    }

    private void ButtonSave_Click(object sender, RoutedEventArgs e)
    {
        if (TaxpayerBox.SelectedItem == null || TaxBox.SelectedItem == null ||
            PeriodBox.SelectedItem == null || MethodBox.SelectedItem == null ||
            string.IsNullOrWhiteSpace(AmountBox.Text) || string.IsNullOrWhiteSpace(StatusBox.Text))
        {
            MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!decimal.TryParse(AmountBox.Text, out decimal amount))
        {
            MessageBox.Show("Неверный формат суммы.");
            return;
        }

        _original.Taxpayer_ID = ((Taxpayers)TaxpayerBox.SelectedItem).Taxpayer_ID;
        _original.Tax_ID = ((Tax)TaxBox.SelectedItem).Tax_ID;
        _original.Period_ID = ((Tax_period)PeriodBox.SelectedItem).Period_ID;
        _original.Method_ID = ((Payment_methods)MethodBox.SelectedItem).Method_ID;
        _original.Amount = amount;
        _original.Status = StatusBox.Text;
        _original.Currency = CurrencyBox.Text;

        Учет_платежей_налоговEntities.GetContext().SaveChanges();

        MessageBox.Show("Изменения сохранены!");
        new PaymentsWindow().Show();
        Close();
    }

    private void ButtonBack_Click(object sender, RoutedEventArgs e)
    {
        new PaymentsWindow().Show();
        Close();
    }
}

/// EditTaxpayerWindow
public partial class EditTaxpayerWindow : Window
{
    private Taxpayers _taxpayer;
    private Taxpayers _copy;

    public EditTaxpayerWindow(Taxpayers taxpayer)
    {
        InitializeComponent();
        _taxpayer = taxpayer;
        _copy = new Taxpayers
        {
            Taxpayer_ID = taxpayer.Taxpayer_ID,
            Full_name = taxpayer.Full_name,
            Type = taxpayer.Type,
            TIN = taxpayer.TIN,
            Address = taxpayer.Address,
            Phone_number = taxpayer.Phone_number
        };
        DataContext = _copy;
    }

    private void ButtonSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_taxpayer.Full_name) ||
            string.IsNullOrWhiteSpace(_taxpayer.Type) ||
            string.IsNullOrWhiteSpace(_taxpayer.TIN) ||
            string.IsNullOrWhiteSpace(_taxpayer.Address) ||
            string.IsNullOrWhiteSpace(_taxpayer.Phone_number))
        {
            MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        _taxpayer.Full_name = _copy.Full_name;
        _taxpayer.Type = _copy.Type;
        _taxpayer.TIN = _copy.TIN;
        _taxpayer.Address = _copy.Address;
        _taxpayer.Phone_number = _copy.Phone_number;

        Учет_платежей_налоговEntities.GetContext().SaveChanges();

        MessageBox.Show("Изменения сохранены!");
        new TaxpayersWindow().Show();
        Close();
    }

    private void ButtonBack_Click(object sender, RoutedEventArgs e)
    {
        new TaxpayersWindow().Show();
        Close();
    }
}

/// EditTaxWindow
public partial class EditTaxWindow : Window
{
    private Tax _tax;
    private Tax _taxCopy;

    public EditTaxWindow(Tax tax)
    {
        InitializeComponent();
        _tax = tax;
        _taxCopy = new Tax
        {
            Tax_ID = tax.Tax_ID,
            Name = tax.Name,
            Rate = tax.Rate,
            Description = tax.Description
        };
        DataContext = _taxCopy;
    }

    private void ButtonSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_taxCopy.Name))
        {
            MessageBox.Show("Поле 'Название' не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        _tax.Name = _taxCopy.Name;
        _tax.Rate = _taxCopy.Rate;
        _tax.Description = _taxCopy.Description;

        Учет_платежей_налоговEntities.GetContext().SaveChanges();

        MessageBox.Show("Изменения сохранены!");
        new TaxWindow().Show();
        Close();
    }

    private void ButtonBack_Click(object sender, RoutedEventArgs e)
    {
        new TaxWindow().Show();
        Close();
    }
}

