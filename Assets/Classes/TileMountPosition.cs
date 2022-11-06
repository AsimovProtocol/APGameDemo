using UnityEngine;

namespace Classes
{
    public class TileMountPosition
    {
        public TileMountPosition(Vector3 position, Quaternion rotation, TileRoom tileRoom)
        {
            Position = position;
            Rotation = rotation;
            this.tileRoom = tileRoom;
        }

        private TileRoom tileRoom;

        public Vector3 Position { get; }
        public Quaternion Rotation { get; }

        public bool Used { get; private set; }
        public GameObject Object { get; private set; }

        public GameObject Populate(GameObject prefab)
        {
            Used = true;
            GameObject gameObject = UnityEngine.Object.Instantiate(prefab, Position, Rotation);
            Object = gameObject;
            gameObject.transform.parent = tileRoom.transform;
            gameObject.transform.Translate(0, 0, 0.5f, gameObject.transform);
            return gameObject;
        }
    }
}