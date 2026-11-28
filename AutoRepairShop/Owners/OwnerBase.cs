using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal static class OwnerBase
    {
        public const int OwnerCount = 200;
        public const int RelationshipStart = 50;
        public const int FactsPerOwner = 2;
        public const int AcquaintedAtStart = 100;
        public const int FactsPerBrand = 200;

        private enum FactKind
        {
            Fact,
            Myth,
            Joke,
            Love,
            Hate
        }

        private static readonly List<string> FirstNames = new List<string>
        {
            "Иван", "Пётр", "Сергей", "Алексей", "Николай",
            "Василий", "Дмитрий", "Андрей", "Михаил", "Юрий",
            "Владимир", "Виктор", "Олег", "Павел", "Артём",
            "Максим", "Роман", "Евгений", "Антон", "Геннадий"
        };

        private static readonly List<string> LastNames = new List<string>
        {
            "Петров", "Сидоров", "Кузнецов", "Смирнов", "Попов",
            "Морозов", "Волков", "Соколов", "Зайцев", "Павлов"
        };

        private static readonly List<CarBrand> Brands = new List<CarBrand>
        {
            CarBrand.Vaz, CarBrand.Vaz, CarBrand.Vaz, CarBrand.Izh, CarBrand.Volga
        };

        private static readonly List<string> StoryOpenings = new List<string>
        {
            "Купил её у тестя в 2003-м,",
            "Брал с рук у соседа по гаражу,",
            "Досталась от отца,",
            "Первую машину выбирал долго,",
            "Взял сразу после армии,",
            "Покупал на последние деньги,",
            "Обменял на «шестёрку» с доплатой,",
            "Нашёл объявление в старой газете,",
            "Привезли из деревни,",
            "Отдал за неё мотоцикл и ящик пива,",
            "Купил на запчасти, а потом восстановил,",
            "Забрал у брата, когда тот пересел на иномарку,",
            "Ездил за ней за триста километров,",
            "Брал в кредит и выплачивал два года,",
            "Купил у деда, который ездил только по праздникам,",
            "Взял в наследство от дяди,",
            "Выиграл в споре,",
            "Купил после того, как старая развалилась,",
            "Приобрёл для работы,",
            "Подарили на свадьбу,"
        };

        private static readonly List<string> StoryDetails = new List<string>
        {
            "и с тех пор она пережила две стройки и один переезд.",
            "помню, как на ней возил рассаду и цемент.",
            "на ней ездили на юг всей семьёй.",
            "зимой стоит в гараже, летом — каждый день на ходу.",
            "пробег давно не сматывается, честно скажу.",
            "по кузову есть рыжики, но мотор — как часы.",
            "дети выросли, а машина осталась.",
            "запчасти беру на рынке, где всё дешевле.",
            "сосед всё время просит продать, а я не отдам.",
            "главное — не гнать, и она не подведёт."
        };

        private static readonly List<string> VazClauses = new List<string>
        {
            "ВАЗ — это конструктор для взрослых.",
            "Классика: всё просто, всё чинится молотком.",
            "Самара — машина для города, а не для деревни."
        };

        private static readonly List<string> IzhClauses = new List<string>
        {
            "Иж — машина с характером, но упрямая.",
            "Мотор Ижа тянет с низов, зато шумит.",
            "Для наших дорог Иж — самый раз."
        };

        private static readonly List<string> VolgaClauses = new List<string>
        {
            "Волга — это баржа: тяжёлая, но идёт ровно.",
            "На Волге главное — не спешить.",
            "Волга не любит суеты, зато умеет возить всё."
        };

        private static readonly List<string> LegendaryFacts = new List<string>
        {
            "Легенда: на заводе в Тольятти до сих пор стоит первый «копеечный» кузов, который не ржавеет.",
            "Легенда: если «Волгу» помыть и поставить в гараж, она молодеет на год.",
            "Легенда: у Иж-2126 есть «счастливый» мотор, который ходит по 500 тысяч.",
            "Легенда: «семёрку» с решёткой от Mercedes видели в кино, и это была заводская машина.",
            "Легенда: у старого механика есть распредвал, с которым Самара едет 200.",
            "Легенда: на Волге можно доехать до моря на одном баке, если не глушить.",
            "Легенда: «шестёрка» из 80-х заводится с пол-оборота даже в минус тридцать.",
            "Легенда: в гараже на окраине стоит ВАЗ-2101 с пробегом 40 тысяч — и хозяин не продаёт.",
            "Легенда: если поставить на Иж мост от 412-го, он перестаёт гудеть.",
            "Легенда: у Волги есть «режим баржи»: если ехать 60, расход падает вдвое."
        };

        private static readonly List<FactKind> VazKinds = new List<FactKind>
        {
            FactKind.Fact, FactKind.Myth, FactKind.Myth, FactKind.Joke, FactKind.Joke,
            FactKind.Myth, FactKind.Fact, FactKind.Love, FactKind.Hate, FactKind.Fact,
            FactKind.Joke, FactKind.Love, FactKind.Fact, FactKind.Joke, FactKind.Myth,
            FactKind.Love, FactKind.Hate, FactKind.Fact, FactKind.Fact, FactKind.Joke,
            FactKind.Myth, FactKind.Love, FactKind.Hate, FactKind.Fact, FactKind.Joke,
            FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love, FactKind.Hate,
            FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love, FactKind.Hate,
            FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love, FactKind.Hate,
            FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love, FactKind.Hate,
            FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love, FactKind.Hate
        };

        private static readonly List<string> VazCores = new List<string>
        {
            "решётку радиатора ВАЗ-2107 рисовали ради «люксового» вида, а не ради сходства с Mercedes",
            "«пятёрка» и «семёрка» разрабатывались одновременно",
            "бамперы 2107 ломались из-за ошибки конструкции",
            "«Жигули» не ломаются — они берут паузу",
            "если из «семёрки» не капает масло — проверь щуп",
            "ВАЗ-2108 делали вместе с Porsche",
            "Самара Т3 с мотором от Porsche 959 существовала — это была раллийная группа B",
            "за то, что любую деталь можно купить в соседнем магазине",
            "зимой печка греет только водителя",
            "«шестёрка» ВАЗ-2106 — самая массовая классика, её до сих пор полно на дорогах",
            "ВАЗ — это не машина, а набор запчастей в форме машины",
            "за звук карбюратора на холодную, когда всё работает",
            "у «копейки» был мотор 1.2 л и 62 л.с.",
            "«копейка» — это не деньги, это состояние души",
            "на классике нельзя ездить зимой",
            "за то, что её можно починить в поле",
            "за ржавчину, которая быстрее тебя",
            "ВАЗ-2108 — первая переднеприводная модель завода",
            "ВАЗ-2107 выпускали с 1982 по 2012 год — тридцать лет",
            "«семёрка» чинится быстрее, чем заправляется",
            "на 2108 стоял мотор от Porsche",
            "на ней можно научиться ремонтировать всё",
            "багажник ржавеет первым",
            "у 2106 был мотор 1.6 л, и это считалось динамикой",
            "ВАЗ не гниёт — он возвращается в природу",
            "первый ВАЗ-2101 сошёл с конвейера в 1970 году",
            "«шестёрка» считалась самой быстрой классикой",
            "классика заводится с пол-оборота, если не считать зимы",
            "за то, что запчасти есть в любом сельском магазине",
            "за то, что зимой стёкла потеют быстрее, чем греется салон",
            "у 2108 поперечный мотор и привод на передние колёса",
            "Самара была первой советской переднеприводной машиной",
            "ВАЗ-2109 — это 2108, который научился возить тёщу",
            "за то, что её можно оставить во дворе и не бояться",
            "за то, что каждый год надо варить пороги",
            "первый ВАЗ-2101 называли «копейкой» за копеечное обслуживание",
            "на 2107 ставили двигатель от «Нивы»",
            "классика едет медленно, зато уверенно",
            "за то, что её все узнают во дворе",
            "за то, что летом в ней жарко, как в бане",
            "у 2109 задняя дверь открывается вверх",
            "Самара не гниёт",
            "2108 — это спорткар для тех, кто не спешит",
            "за то, что в неё влезает вся семья",
            "за то, что её надо мыть чаще, чем чинить",
            "ВАЗ-2110 получил передние стойки МакФерсон",
            "на 2110 ставили мотор от иномарки",
            "«десятка» — это когда комфорт догнал классику",
            "за то, что она едет тише «семёрки»",
            "за то, что запчасти на неё дороже"
        };

        private static readonly List<FactKind> IzhKinds = new List<FactKind>
        {
            FactKind.Joke, FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love,
            FactKind.Hate, FactKind.Fact, FactKind.Joke, FactKind.Myth, FactKind.Love,
            FactKind.Hate, FactKind.Fact, FactKind.Fact, FactKind.Joke, FactKind.Myth,
            FactKind.Love, FactKind.Hate, FactKind.Fact, FactKind.Fact, FactKind.Joke,
            FactKind.Myth, FactKind.Love, FactKind.Hate, FactKind.Fact, FactKind.Joke,
            FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love, FactKind.Hate,
            FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love, FactKind.Hate,
            FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love, FactKind.Hate,
            FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love, FactKind.Hate,
            FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love, FactKind.Hate
        };

        private static readonly List<string> IzhCores = new List<string>
        {
            "Иж — это не «Ода», а «Ода проблемам»",
            "у большинства Ижей редуктор моста гудит — это не поломка, а характер",
            "Иж-2126 — копия Москвича",
            "на Иже главное — не скорость, а чтобы доехать",
            "за клиренс и за то, что лезет туда, куда Самара не рискнёт",
            "за шум в салоне на скорости выше 80",
            "Иж-2715 — фургон, и он до сих пор работает у дачников",
            "Иж не глохнет, он задумывается",
            "запчасти на Иж сняли с производства",
            "за то, что его не жалко поцарапать",
            "печка дует холодом, пока не прогреется",
            "мотор Ижа славится тягой с низов, но не любит высоких оборотов",
            "Иж-2126 выпускали с 1990 по 2005 год",
            "Иж — это когда едешь и слышишь каждую гайку",
            "на Иже нельзя далеко",
            "за простую конструкцию и дешёвые запчасти",
            "кузов гниёт быстрее, чем кончается масло",
            "у Иж-2715 грузоподъёмность около 500 кг",
            "Иж-2126 — последняя самостоятельная модель завода",
            "у Ижа обзор как у трактора — зато всё видно",
            "Иж не едет больше 90",
            "в нём не страшно возить мешки",
            "двери закрываются только с третьего раза",
            "у Иж-2715 задняя дверь открывается как у фургона",
            "Иж — это конструктор «сделай сам» с завода",
            "Иж-2126 собирали на агрегатах «Москвича-412»",
            "у Ижа с завода стоял мотор от ВАЗ-2106",
            "Иж едет даже на трёх цилиндрах",
            "за то, что в фургон влезает холодильник",
            "за то, что запчасти искать дольше, чем чинить",
            "Иж-2715 выпускали более тридцати лет",
            "Иж не ржавеет",
            "печка на Иже работает как кондиционер — только наоборот",
            "за то, что его не жалко",
            "двери надо закрывать с разбегу",
            "Иж-2126 шёл с мотором 1.6 и пятиступенчатой коробкой",
            "Иж-2126 — это перелицованный «Москвич-2141»",
            "Иж — это «Ода» без оваций",
            "за то, что он прощает плохие дороги",
            "за то, что его надо чинить в поле",
            "Иж-2715 возил продукты по деревням десятилетиями",
            "на Иже нельзя ездить зимой",
            "Иж — автомобиль, который всегда доедет, если его толкать",
            "за то, что он не боится ям",
            "за то, что шумно в салоне",
            "Иж-2126 получил передние дисковые тормоза",
            "Иж сняли с производства в 2000-х",
            "«Ода» и «Ода проблемам» — это про один и тот же Иж",
            "за то, что он дешевле любой иномарки",
            "за то, что на него трудно найти кузовщину"
        };

        private static readonly List<FactKind> VolgaKinds = new List<FactKind>
        {
            FactKind.Joke, FactKind.Fact, FactKind.Joke, FactKind.Myth, FactKind.Love,
            FactKind.Hate, FactKind.Fact, FactKind.Joke, FactKind.Myth, FactKind.Love,
            FactKind.Hate, FactKind.Fact, FactKind.Fact, FactKind.Joke, FactKind.Myth,
            FactKind.Love, FactKind.Hate, FactKind.Fact, FactKind.Fact, FactKind.Joke,
            FactKind.Myth, FactKind.Love, FactKind.Hate, FactKind.Fact, FactKind.Joke,
            FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love, FactKind.Hate,
            FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love, FactKind.Hate,
            FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love, FactKind.Hate,
            FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love, FactKind.Hate,
            FactKind.Fact, FactKind.Myth, FactKind.Joke, FactKind.Love, FactKind.Hate
        };

        private static readonly List<string> VolgaCores = new List<string>
        {
            "«Волгу» прозвали баржей — она и правда дрейфует не спеша",
            "ГАЗ-24 официально назывался «Волга», а «баржу» придумали водители",
            "если из «Волги» перестало течь масло — значит, оно закончилось",
            "Волга не едет быстро",
            "на трассе она идёт как корабль",
            "за расход — ведро бензина на сотню",
            "ГАЗ-3110 выпускали с 1997 по 2004 год",
            "Волга — единственная машина, где задний диван удобнее передних сидений",
            "ГУР на Волге — роскошь",
            "за багажник, в который влезает всё",
            "каждая поездка — это плановое ТО",
            "задний мост Волги подвывает почти у всех",
            "ГАЗ-24 выпускали с 1970 по 1986 год",
            "Волга — это не машина, а образ жизни",
            "Волга не влезает в гараж",
            "её уважают на дороге",
            "заправлять чаще, чем чинить",
            "у ГАЗ-24 стоял двигатель ЗМЗ-24 объёмом 2.4 литра",
            "ГАЗ-3102 получил форкамерный двигатель и считался начальственным",
            "Волга — это когда обгоняешь, а тебе сигналят из уважения",
            "Волга не ломается",
            "в ней не укачивает",
            "парковаться на ней — отдельное искусство",
            "у ГАЗ-3110 был инжекторный мотор ЗМЗ-406",
            "на Волге не ездят — на ней плавают",
            "ГАЗ-24 был первой Волгой с автоматом в экспорте",
            "Волга — машина для начальства",
            "Волга — это дом на колёсах, только с расходом дома",
            "за то, что в ней не страшно на трассе",
            "за то, что её надо прогревать полчаса",
            "ГАЗ-3102 оснащали форкамерным двигателем для экономии",
            "на Волге стоял мотор от «Чайки»",
            "Волга паркуется там, где ей вздумается",
            "за то, что её уважают гаишники",
            "за то, что каждое ТО дороже, чем на ВАЗ",
            "ГАЗ-24 сходил с конвейера более миллиона экземпляров",
            "Волга — только для такси",
            "Волга — это корабль, только без воды",
            "за то, что она вмещает всю семью и тёщу",
            "за то, что её трудно парковать",
            "ГАЗ-3102 выпускали малыми сериями для руководства",
            "Волга не гниёт",
            "Волга ест бензин, как будто он бесплатный",
            "за то, что она не боится трассы",
            "за то, что запчасти дороже вазовских",
            "у ГАЗ-3110 была пятиступенчатая коробка",
            "на Волге стоит мотор от «Газели»",
            "Волга едет, пока в неё льют бензин",
            "за то, что она внушает уважение",
            "за то, что каждое ТО — как ремонт"
        };

        private static readonly List<string> VazFacts = BuildFacts(VazKinds, VazCores);
        private static readonly List<string> IzhFacts = BuildFacts(IzhKinds, IzhCores);
        private static readonly List<string> VolgaFacts = BuildFacts(VolgaKinds, VolgaCores);

        public static List<Owner> CreateAll()
        {
            var owners = new List<Owner>();

            for (int i = 0; i < OwnerCount; i++)
            {
                string firstName = FirstNames[i % FirstNames.Count];
                string lastName = LastNames[(i / FirstNames.Count) % LastNames.Count];

                CarBrand brand = Brands[i % Brands.Count];
                IReadOnlyList<string> models = CarNames.GetNames(brand);
                string model = models[i % models.Count];

                var nationality = (Nationality)(i % 10);
                string story = Accents.DecorateStory(CreateStory(brand, model, i), nationality);

                owners.Add(new Owner(
                    i,
                    firstName + " " + lastName,
                    brand,
                    model,
                    story,
                    CreateFacts(brand, model, i, nationality),
                    RelationshipStart,
                    nationality,
                    i < AcquaintedAtStart));
            }

            return owners;
        }

        public static List<string> GetFactPool(CarBrand brand)
        {
            switch (brand)
            {
                case CarBrand.Izh:
                    return IzhFacts;

                case CarBrand.Volga:
                    return VolgaFacts;

                default:
                    return VazFacts;
            }
        }

        private static string CreateStory(CarBrand brand, string model, int index)
        {
            string opening = StoryOpenings[index % StoryOpenings.Count];
            string detail = StoryDetails[(index / StoryOpenings.Count) % StoryDetails.Count];

            return opening + " " + detail + " " + GetClause(brand, index) + " " + model + ".";
        }

        private static string GetClause(CarBrand brand, int index)
        {
            switch (brand)
            {
                case CarBrand.Izh:
                    return IzhClauses[index % IzhClauses.Count];

                case CarBrand.Volga:
                    return VolgaClauses[index % VolgaClauses.Count];

                default:
                    return VazClauses[index % VazClauses.Count];
            }
        }

        private static IReadOnlyList<string> CreateFacts(
            CarBrand brand,
            string model,
            int index,
            Nationality nationality)
        {
            List<string> pool = GetFactPool(brand);
            var facts = new List<string>();

            int first = index % pool.Count;

            facts.Add(Accents.Distort(pool[first], nationality));

            List<string> modelFacts = ModelFacts.GetFacts(model);

            if (modelFacts.Count > 0)
            {
                string modelFact = modelFacts[index % modelFacts.Count];

                facts.Add(Accents.GetModelAttitude(nationality, model) + " " +
                    Accents.Distort(modelFact, nationality));
            }
            else
            {
                int second = (index * 7 + 3) % pool.Count;

                facts.Add(Accents.Distort(pool[second], nationality));
            }

            if (index % 100 == 0)
            {
                string legend = LegendaryFacts[index % LegendaryFacts.Count];

                facts.Add(Accents.Distort(legend, nationality));
            }

            return facts;
        }

        private static List<string> BuildFacts(List<FactKind> kinds, List<string> cores)
        {
            var facts = new List<string>();

            for (int i = 0; i < cores.Count; i++)
            {
                foreach (string pattern in GetPatterns(kinds[i]))
                {
                    facts.Add(string.Format(pattern, cores[i]));
                }
            }

            return facts;
        }

        private static string[] GetPatterns(FactKind kind)
        {
            switch (kind)
            {
                case FactKind.Myth:
                    return new[]
                    {
                        "Миф: {0}.",
                        "Говорят, что {0} — но это миф.",
                        "В народе верят: {0}.",
                        "Слышал байку: {0}."
                    };

                case FactKind.Joke:
                    return new[]
                    {
                        "Шутка: {0}.",
                        "В гараже шутят: {0}.",
                        "Насмешили: {0}.",
                        "Мужики говорят: {0}."
                    };

                case FactKind.Love:
                    return new[]
                    {
                        "Люблю: {0}.",
                        "Хорошее: {0}.",
                        "За что ценю: {0}.",
                        "Плюс: {0}."
                    };

                case FactKind.Hate:
                    return new[]
                    {
                        "Ненавижу: {0}.",
                        "Минус: {0}.",
                        "Бесит: {0}.",
                        "Не люблю: {0}."
                    };

                default:
                    return new[]
                    {
                        "Факт: {0}.",
                        "На самом деле {0}.",
                        "Знающие люди говорят: {0}.",
                        "Механик подтвердит: {0}."
                    };
            }
        }
    }
}
