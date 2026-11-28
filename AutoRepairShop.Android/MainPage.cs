using AutoRepairShop;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace AutoRepairShop.Android;

public class MainPage : ContentPage
{
    private const decimal Rent = 6_000m;
    private const decimal Goal = 5_000_000m;

    private readonly IRandomSource _random = new SystemRandomSource();
    private readonly VerticalStackLayout _layout = new VerticalStackLayout { Padding = 16, Spacing = 8 };
    private readonly Label _message = new Label { TextColor = Colors.DarkOrange };

    private List<Owner> _owners = new();
    private readonly List<Car> _queue = new();
    private readonly List<DeliveryOrder> _deliveries = new();
    private readonly CarFactory _carFactory;
    private readonly PartsFactory _partsFactory;
    private readonly UpgradeShop _upgradeShop;
    private Warehouse _warehouse;
    private Garage _garage;
    private CashRegister _cash;
    private ServiceDevelopment _development;
    private PartsShop _shop;
    private Car _current;
    private Owner _currentOwner;
    private string _mode = "day";
    private int _day = 1;

    public MainPage()
    {
        Title = "Гараж Народный";
        Content = new ScrollView { Content = _layout };

        _carFactory = new CarFactory(_random);
        _partsFactory = new PartsFactory();
        _cash = new CashRegister();
        _development = new ServiceDevelopment();
        _warehouse = new WarehouseFactory(_partsFactory).Create();
        _garage = new Garage(_warehouse, _random);
        _shop = new PartsShop(_cash);
        _upgradeShop = new UpgradeShop(_cash, _development);

        _layout.Children.Add(_message);

        LoadOrStart();
        StartDay();
        Render();
    }

    private void LoadOrStart()
    {
        _owners = OwnerBase.CreateAll();

        if (GameSave.TryRead(GameSave.GetPath(1), out GameSave save) == false)
        {
            return;
        }

        _cash = new CashRegister(save.Money);
        _development = new ServiceDevelopment(save.Levels);
        _day = save.Day;

        int capacity = Warehouse.DefaultCapacity
            + UpgradeCatalog.Get(UpgradeType.Warehouse).EffectPerLevel
            * _development.GetLevel(UpgradeType.Warehouse);

        _warehouse = new WarehouseFactory(_partsFactory).Create(capacity, save.Stock);
        _garage = new Garage(_warehouse, _random);
        _shop = new PartsShop(_cash);
        _upgradeShop = new UpgradeShop(_cash, _development);

        foreach (var pair in save.OwnerScores)
        {
            if (pair.Key >= 0 && pair.Key < _owners.Count)
            {
                _owners[pair.Key].RestoreRelationship(pair.Value);
            }
        }

        foreach (var pair in save.OwnerFlags)
        {
            if (pair.Key >= 0 && pair.Key < _owners.Count)
            {
                int referrer = save.OwnerReferrers.TryGetValue(pair.Key, out int stored) ? stored : -1;
                _owners[pair.Key].RestoreFlags((pair.Value & 1) != 0, (pair.Value & 2) != 0, referrer);
            }
        }

        foreach (Car car in save.QueueCars)
        {
            _queue.Add(car);
        }
    }

    private void StartDay()
    {
        if (_queue.Count > 0)
        {
            return;
        }

        var eligible = _owners.Where(owner => owner.IsAngry == false && owner.IsAcquainted).ToList();

        if (eligible.Count == 0)
        {
            return;
        }

        int count = 1 + _random.NextInclusive(0, CarsQueue.DefaultMaxCarsPerDay);

        for (int i = 0; i < count; i++)
        {
            Owner owner = eligible[_random.Next(0, eligible.Count)];
            Car car = _carFactory.CreateForModel(owner.Brand, owner.Model, GetSeason(_day));
            car.AssignOwner(owner.Id);
            _queue.Add(car);
        }
    }

    private static Season GetSeason(int day) => (Season)(((day - 1) / 15) % 4);

    private void Render()
    {
        while (_layout.Children.Count > 1)
        {
            _layout.Children.RemoveAt(1);
        }

        AddHeader();

        if (_mode == "shop")
        {
            RenderShop();
        }
        else if (_mode == "owners")
        {
            RenderOwners();
        }
        else if (_current != null)
        {
            RenderCar();
        }
        else
        {
            RenderDay();
        }
    }

    private void AddHeader()
    {
        _layout.Children.Add(new Label
        {
            Text = $"День {_day} · {GetSeason(_day)}",
            FontSize = 22,
            FontAttributes = FontAttributes.Bold
        });

        _layout.Children.Add(new Label
        {
            Text = $"В кассе: {_cash.Money:N0} ₽ из {Goal:N0} ₽",
            FontSize = 16
        });

        _layout.Children.Add(new Label
        {
            Text = $"В очереди: {_queue.Count} · склад: {_warehouse.TotalQuantity}/{_warehouse.Capacity}",
            FontSize = 14,
            TextColor = Colors.Gray
        });
    }

    private void RenderDay()
    {
        AddButton("Принять машину", () =>
        {
            if (_queue.Count == 0)
            {
                Show("Очередь пуста — заканчивайте день.");
                return;
            }

            _current = _queue[0];
            _queue.RemoveAt(0);
            _currentOwner = FindOwner(_current.OwnerId);
            Render();
        }, _queue.Count > 0);

        AddButton("Магазин запчастей", () =>
        {
            _mode = "shop";
            Render();
        });

        AddButton("Владельцы", () =>
        {
            _mode = "owners";
            Render();
        });

        AddButton("Закончить день", EndDay);
    }

    private void RenderCar()
    {
        Owner owner = _currentOwner;

        if (owner != null)
        {
            _layout.Children.Add(new Label
            {
                Text = owner.GetGreeting() + " " + owner.Name + " (" + owner.GetStatus() + ")",
                FontSize = 16,
                TextColor = Colors.Blue
            });
        }

        _layout.Children.Add(new Label
        {
            Text = _current.Name,
            FontSize = 20,
            FontAttributes = FontAttributes.Bold
        });

        if (_current.NeedsTireChange)
        {
            AddButton("Шиномонтаж (+2 000 ₽)", () =>
            {
                _current.CompleteTireChange();
                _cash.Apply(GameBalance.TireChangePrice);
                Show("Шины клиента переобуты.");
                Render();
            });
        }

        foreach (Detail detail in _current.GetAllDetails().Where(item => item.IsBroken).ToList())
        {
            _layout.Children.Add(new Label
            {
                Text = $"{detail.Name} ({detail.BrandName}) — {detail.GetPayout(1m):N0} ₽",
                FontSize = 15
            });

            AddButton("Заменить", () => Replace(detail), _warehouse.GetQuantityDetail(detail.Type) > 0);

            if (detail.IsRepairable)
            {
                AddButton("Ремонт без замены", () => Repair(detail));
            }
        }

        if (_current.CarIsBroken() == false)
        {
            AddButton("Машина готова", FinishCar);
        }

        AddButton("Отказать (штраф)", () =>
        {
            RepairReport report = _garage.RefuseFurtherRepairs(0m, _current);
            _cash.Apply(report.Total);
            React(-12, "Клиент ушёл недовольным.");
        });
    }

    private void Replace(Detail detail)
    {
        DetailRepairReport report = _garage.RepairDetail(detail.Type, _current, 1m);

        _cash.Apply(report.Charge * (_currentOwner == null ? 1m : _currentOwner.GetPayMultiplier()));

        if (report.Outcome == RepairOutcome.ReplacedBroken)
        {
            React(8, "Деталь заменена.");
        }
        else
        {
            Show("Деталь была исправна — без выплаты.");
        }

        Render();
    }

    private void Repair(Detail detail)
    {
        DetailRepairReport report = _garage.RepairWithoutReplacement(detail.Type, _current, 1m);

        _cash.Apply(report.Charge);
        React(8, "Ремонт без замены выполнен.");
        Render();
    }

    private void FinishCar()
    {
        React(8, "Машина обслужена.");
    }

    private void React(int delta, string message)
    {
        if (_currentOwner != null)
        {
            _currentOwner.ChangeRelationship(delta);
            message += " " + _currentOwner.GetFarewell();
        }

        _current = null;
        _currentOwner = null;
        Show(message);
        Render();
    }

    private void RenderShop()
    {
        foreach (PartTemplate template in DetailsCatalog.Templates
                     .Where(item => item.Category != PartCategory.Material))
        {
            PartTemplate captured = template;

            decimal price = captured.GetPrice(DetailsCatalog.StandardBrand);
            int quantity = _warehouse.GetQuantityDetail(captured.Type);

            AddButton($"{captured.Name} — {price:N0} ₽ ({quantity} шт.)", () => Buy(captured));
        }

        AddButton("Назад", () =>
        {
            _mode = "day";
            Render();
        });
    }

    private void Buy(PartTemplate template)
    {
        PurchaseReport report = _shop.Buy(template.Type, DetailsCatalog.StandardBrand);

        if (report.Outcome != PurchaseOutcome.Bought)
        {
            Show("Не хватает денег.");
            return;
        }

        _deliveries.Add(new DeliveryOrder(template.Type, DetailsCatalog.StandardBrand.Name, _day + 1));
        Show($"Заказано: {template.Name}. Приедет завтра.");
        Render();
    }

    private void RenderOwners()
    {
        foreach (Owner owner in _owners.Where(item => item.IsAcquainted).OrderBy(item => item.Relationship).Take(40))
        {
            Owner captured = owner;

            _layout.Children.Add(new Label
            {
                Text = $"{captured.Name} — {captured.GetStatus()} ({captured.Relationship})",
                FontSize = 14,
                TextColor = captured.IsAngry ? Colors.Red : Colors.Black
            });
        }

        AddButton("Назад", () =>
        {
            _mode = "day";
            Render();
        });
    }

    private void EndDay()
    {
        _cash.Apply(-Rent);
        Deliver();

        Save();
        _day++;
        StartDay();
        Show("День закончен.");
        Render();
    }

    private void Deliver()
    {
        var remaining = new List<DeliveryOrder>();

        foreach (DeliveryOrder order in _deliveries)
        {
            if (order.ArrivalDay > _day)
            {
                remaining.Add(order);
                continue;
            }

            Brand brand = DetailsCatalog.Brands.First(item => item.Name == order.BrandName);
            PartTemplate template = DetailsCatalog.GetTemplate(order.Type);

            if (_warehouse.TryAdd(new Detail(template, brand)) == false)
            {
                remaining.Add(new DeliveryOrder(order.Type, order.BrandName, _day + 1));
            }
        }

        _deliveries.Clear();
        _deliveries.AddRange(remaining);
    }

    private void Save()
    {
        var levels = new Dictionary<UpgradeType, int>();
        var stock = new Dictionary<DetailType, int>();
        var scores = new Dictionary<int, int>();
        var flags = new Dictionary<int, int>();
        var referrers = new Dictionary<int, int>();

        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            levels[type] = _development.GetLevel(type);
        }

        foreach (PartTemplate template in DetailsCatalog.Templates)
        {
            stock[template.Type] = _warehouse.GetQuantityDetail(template.Type);
        }

        foreach (Owner owner in _owners)
        {
            scores[owner.Id] = owner.Relationship;
            flags[owner.Id] = (owner.HasReferred ? 1 : 0) | (owner.IsAcquainted ? 2 : 0);
            referrers[owner.Id] = owner.ReferrerId;
        }

        var save = new GameSave(
            _cash.Money,
            _day + 1,
            levels,
            stock,
            GameSave.CreateEmptyShift(),
            0,
            new List<CourseType>(),
            0m,
            _deliveries.ToList(),
            _queue.ToList(),
            null,
            scores,
            flags,
            referrers,
            null);

        try
        {
            GameSave.Write(GameSave.GetPath(1), save);
        }
        catch (IOException)
        {
        }
    }

    private Owner FindOwner(int id)
    {
        return _owners.FirstOrDefault(owner => owner.Id == id);
    }

    private void AddButton(string text, Action action, bool enabled = true)
    {
        var button = new Button
        {
            Text = text,
            IsEnabled = enabled,
            HorizontalOptions = LayoutOptions.Fill
        };

        button.Clicked += (sender, args) => action();
        _layout.Children.Add(button);
    }

    private void Show(string message) => _message.Text = message;
}
