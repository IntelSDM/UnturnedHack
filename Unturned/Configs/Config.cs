using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hag.Configs
{
    class Config
    {
        
        public Colours Colours = new Colours();
        public Hag.Configs.Menu Menu = new Hag.Configs.Menu();
        public Hag.Configs.Zombie Zombie = new Hag.Configs.Zombie();
        public Hag.Configs.Player Player = new Hag.Configs.Player();
        public Hag.Configs.EntityAimbot ZombieAimbot = new Hag.Configs.EntityAimbot();
        public Hag.Configs.EntityAimbot PlayerAimbot = new Hag.Configs.EntityAimbot();
        public Hag.Configs.Aimbot Aimbot = new Hag.Configs.Aimbot();
        public Weapon Weapon = new Weapon();
        public Friends Friends = new Friends();
        public FriendlyPlayer FriendlyPlayer = new FriendlyPlayer();
        public Vehicle Vehicle = new Vehicle();
        public Item Item = new Item();
        public Movement Movement = new Movement();
        public VehicleMisc VehicleMisc = new VehicleMisc();

        public ItemFilter MagazineFilter = new ItemFilter();
        public ItemFilter AmmoFilter = new ItemFilter();
        public ItemFilter BarrelFilter = new ItemFilter();
        public ItemFilter OpticFilter = new ItemFilter();
        public ItemFilter BackpackFilter = new ItemFilter();
        public ItemFilter ClothingFilter = new ItemFilter();
        public ItemFilter FuelFilter = new ItemFilter();
        public ItemFilter MedicalFilter = new ItemFilter();
        public ItemFilter MeleeFilter = new ItemFilter();
        public ItemFilter ThrowableFilter = new ItemFilter();
        public ItemFilter FoodAndWaterFilter = new ItemFilter();
        public ItemFilter GunFilter = new ItemFilter();
        public ItemFilter FarmFilter = new ItemFilter();
        public ItemFilter ExplosivesFilter = new ItemFilter();
        public ItemFilter SupplyFilter = new ItemFilter();
    }
}
