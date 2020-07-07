using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPattern
{
    class Program
    {
        static void Main(string[] args) {
            Gun shotgun = new Gun(new MultipleShootBehaviour(7), new SingleBulletAmmoStorageBehaviour());
            Gun pistol = new Gun(new SingleShootBehaviour(), new MagazineAmmoStorageBehaviour(5));
            Gun rifle = new Gun(new BurstShootBehaviour(), new MagazineAmmoStorageBehaviour(8));

            TestShooting(shotgun);
            Console.WriteLine();
            TestShooting(pistol);
            Console.WriteLine();
            TestShooting(rifle);
            Console.WriteLine();
            
        }

        static void TestShooting(Gun gun) {
            for (int i = 0; i < 9; i++)
                gun.Shoot();
            gun.Reload();
            for (int i = 0; i < 9; i++)
                gun.Shoot();
        }
    }

    class Gun
    {
        IShootBehaviour shootBehaviour;
        IAmmoStorageBehaviour ammoStorageBehaviour;
        int magazineAmmo;

        public Gun(IShootBehaviour shootBehaviour, IAmmoStorageBehaviour ammoStorageBehaviour) {
            this.shootBehaviour = shootBehaviour;
            this.ammoStorageBehaviour = ammoStorageBehaviour;
        }

        public void Shoot() {
            shootBehaviour.Shoot(ammoStorageBehaviour);
        }

        public void Reload() {
            ammoStorageBehaviour.Reload();
        }
    }

    interface IShootBehaviour
    {
        void Shoot(IAmmoStorageBehaviour ammoStorageBehaviour);
    }

    class MultipleShootBehaviour : IShootBehaviour
    {
        int pelletsPerShot;

        public void Shoot(IAmmoStorageBehaviour ammoStorageBehaviour) {
            if (ammoStorageBehaviour.GetBullet()) {
                Console.WriteLine($"Fired {pelletsPerShot} pellets.");
            } else {
                Console.WriteLine($"Couldn't fire.");
            }
        }

        public MultipleShootBehaviour(int pelletsPerShot) {
            this.pelletsPerShot = pelletsPerShot;
        }
    }

    class SingleShootBehaviour : IShootBehaviour
    {
        public void Shoot(IAmmoStorageBehaviour ammoStorageBehaviour) {
            if (ammoStorageBehaviour.GetBullet()) {
                Console.WriteLine($"Fired single bullet.");
            } else {
                Console.WriteLine($"Couldn't fire.");
            }
            
        }
    }

    class BurstShootBehaviour : IShootBehaviour
    {
        public void Shoot(IAmmoStorageBehaviour ammoStorageBehaviour) {
            int burstSize = 0;
            while (ammoStorageBehaviour.GetBullet()) {
                burstSize++;
                if (burstSize == 3)
                    break;
            }
            if(burstSize > 0) {
                Console.WriteLine($"Fired a burst of {burstSize} bullets.");
            } else {
                Console.WriteLine($"Couldn't fire.");
            }
        }
    }

    interface IAmmoStorageBehaviour
    {
        void Reload();
        bool GetBullet();
    }

    class MagazineAmmoStorageBehaviour : IAmmoStorageBehaviour
    {
        int magazineSize;
        int currentAmmo;

        public MagazineAmmoStorageBehaviour(int magazineSize) {
            this.magazineSize = magazineSize;
            currentAmmo = magazineSize;
        }

        public void Reload() {
            currentAmmo = magazineSize;
            Console.WriteLine($"Changed magazine({magazineSize}).");
        }

        public bool GetBullet() {
            if(currentAmmo > 0) {
                currentAmmo--;
                Console.WriteLine("Bullet removed from magazine.");
                return true;
            } else {
                Console.WriteLine("No more bullets left in magazine.");
                return false;
            }
        }
    }

    class SingleBulletAmmoStorageBehaviour : IAmmoStorageBehaviour
    {
        bool loaded;

        public void Reload() {
            loaded = true;
            Console.WriteLine("Reloaded bullet.");
        }
        
        public SingleBulletAmmoStorageBehaviour() {
            loaded = true;
        }

        public bool GetBullet() {
            if (loaded) {
                loaded = false;
                Console.WriteLine("Bullet removed.");
                return true;
            } else {
                Console.WriteLine("Weapon is not loaded.");
                return false;
            }
        }
    }

}
