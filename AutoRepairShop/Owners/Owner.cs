using System;
using System.Collections.Generic;

namespace AutoRepairShop
{
    internal class Owner
    {
        public const int MinRelationship = 0;
        public const int MaxRelationship = 100;
        public const int AngryThreshold = 20;
        public const int FriendThreshold = 80;
        public const int PleasantThreshold = 60;
        public const int NeutralThreshold = 40;

        public Owner(
            int id,
            string name,
            CarBrand brand,
            string model,
            string story,
            IReadOnlyList<string> facts,
            int relationship,
            Nationality nationality,
            bool isAcquainted = true)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Номер владельца не может быть отрицательным.");
            }

            if (facts == null || facts.Count == 0)
            {
                throw new ArgumentException("У владельца должны быть факты о машине.", nameof(facts));
            }

            Id = id;
            Name = name;
            Brand = brand;
            Model = model;
            Story = story;
            Facts = facts;
            Relationship = Clamp(relationship);
            Nationality = nationality;
            IsAcquainted = isAcquainted;
        }

        public int Id { get; }

        public string Name { get; }

        public CarBrand Brand { get; }

        public string Model { get; }

        public string Story { get; }

        public IReadOnlyList<string> Facts { get; }

        public int Relationship { get; private set; }

        public Nationality Nationality { get; }

        public bool IsAcquainted { get; private set; }

        public bool HasReferred { get; private set; }

        public int ReferrerId { get; private set; } = -1;

        public bool IsAngry => Relationship < AngryThreshold;

        public bool IsFriend => Relationship >= FriendThreshold;

        public void ChangeRelationship(int delta) => Relationship = Clamp(Relationship + delta);

        public void RestoreRelationship(int value) => Relationship = Clamp(value);

        public void MakeAcquainted() => IsAcquainted = true;

        public void MarkReferred() => HasReferred = true;

        public void SetReferrer(int referrerId) => ReferrerId = referrerId;

        public void ClearReferrer() => ReferrerId = -1;

        public void Unacquaint() => IsAcquainted = false;

        public void RestoreFlags(bool hasReferred, bool isAcquainted, int referrerId)
        {
            HasReferred = hasReferred;
            IsAcquainted = isAcquainted;
            ReferrerId = referrerId;
        }

        public string GetGreeting()
        {
            if (IsAngry)
            {
                return "Больше не приеду.";
            }

            if (IsFriend)
            {
                return Accents.GetFriendlyGreeting(Nationality);
            }

            if (Relationship < NeutralThreshold)
            {
                return Accents.GetColdGreeting(Nationality);
            }

            return Accents.GetGreeting(Nationality);
        }

        public string GetPraise() => Accents.GetPraise(Nationality);

        public string GetComplaint() => Accents.GetComplaint(Nationality);

        public string GetFarewell()
        {
            if (IsAngry)
            {
                return "Больше не приеду.";
            }

            if (IsFriend || Relationship >= PleasantThreshold)
            {
                return Accents.GetFriendlyFarewell(Nationality);
            }

            if (Relationship < NeutralThreshold)
            {
                return Accents.GetColdFarewell(Nationality);
            }

            return Accents.GetGreeting(Nationality);
        }

        public decimal GetPayMultiplier()
        {
            if (IsFriend)
            {
                return 1.10m;
            }

            if (Relationship >= PleasantThreshold)
            {
                return 1.05m;
            }

            if (Relationship >= NeutralThreshold)
            {
                return 1.00m;
            }

            return 0.90m;
        }

        public string GetStatus()
        {
            if (IsAngry)
            {
                return "в ярости";
            }

            if (IsFriend)
            {
                return "друг";
            }

            if (Relationship >= PleasantThreshold)
            {
                return "приятель";
            }

            if (Relationship >= NeutralThreshold)
            {
                return "нейтрально";
            }

            return "недоволен";
        }

        private static int Clamp(int value)
        {
            if (value < MinRelationship)
            {
                return MinRelationship;
            }

            if (value > MaxRelationship)
            {
                return MaxRelationship;
            }

            return value;
        }
    }
}
