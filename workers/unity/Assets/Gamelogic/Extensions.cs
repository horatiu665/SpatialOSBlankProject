namespace Assets.Gamelogic
{
    using Improbable.Math;
    using Improbable.Player;
    using Improbable.Worker;
    using UnityEngine;

    public static class Extensions
    {
        public static Vector3 ToVector3(this Coordinates coordinates)
        {
            return new Vector3((float)coordinates.X, (float)coordinates.Y, (float)coordinates.Z);
        }

        public static Coordinates ToCoordinates(this Vector3 vector)
        {
            return new Coordinates(vector.x, vector.y, vector.z);
        }

        public static Quaternion ToQuaternion(this QuaternionData quaternion)
        {
            return new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }

        public static QuaternionData ToQuaternionData(this Quaternion quaternion)
        {
            return new QuaternionData(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }

        public static TransformData ToTransformData(this Transform transform)
        {
            return new TransformData(transform.position.ToCoordinates(), transform.rotation.ToQuaternionData());
        }

        public static void UpdateFromTransformData(this Transform transform, TransformData data)
        {
            transform.position = data.position.ToVector3();
            transform.rotation = data.rotation.ToQuaternion();
        }

        public static bool HasChangedBy(this TransformData data, TransformData comparison, double positionThreshold, double rotationThreshold)
        {
            return (data.position.HasChangedBy(comparison.position, positionThreshold)) || (data.rotation.HasChangedBy(comparison.rotation, rotationThreshold));
        }

        public static bool HasChangedBy(this Coordinates data, Coordinates comparison, double positionThreshold)
        {
            return (data - comparison).SquareMagnitude() > positionThreshold * positionThreshold;
        }

        public static bool HasChangedBy(this QuaternionData data, QuaternionData comparison, double rotationThreshold)
        {
            return (Mathf.Abs(data.x - comparison.x) > rotationThreshold) ||
                   (Mathf.Abs(data.y - comparison.y) > rotationThreshold) ||
                   (Mathf.Abs(data.z - comparison.z) > rotationThreshold) ||
                   (Mathf.Abs(data.w - comparison.w) > rotationThreshold);
        }

        public static TransformData DefaultTransformData(this SnapshotEntity entity)
        {
            return new TransformData(Coordinates.ZERO, new QuaternionData(0f, 0f, 0f, 0f));
        }
    }
}