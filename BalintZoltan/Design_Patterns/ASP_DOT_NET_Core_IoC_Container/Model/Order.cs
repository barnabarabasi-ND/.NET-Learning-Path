namespace ASP_DOT_NET_Core_IoC_Container.Models;
public class Order
{
    private int _id;
    private string _customerEmail = string.Empty;
    private string _customerPhone = string.Empty;

    public int Id
    {
        get => _id;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("Order ID must be positive.", nameof(Id));
            }

            _id = value;
        }
    }

    public string CustomerEmail
    {
        get => _customerEmail;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if (!value.Contains('@'))
            {
                throw new ArgumentException("Invalid email format.", nameof(CustomerEmail));
            }

            _customerEmail = value;
        }
    }

    public string CustomerPhone
    {
        get => _customerPhone;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _customerPhone = value;
        }
    }
}

