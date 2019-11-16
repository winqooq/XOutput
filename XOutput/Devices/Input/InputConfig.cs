namespace XOutput.Devices.Input
{
    public class InputConfig
    {
        /// <summary>
        /// Enables the force feedback for the controller.
        /// </summary>
        public bool ForceFeedback { get; set; }

        public string Id { get; set; }

        public InputConfig()
        {
            Id = "";
            ForceFeedback = false;
        }

        public InputConfig(string id)
        {
            Id = id;
            ForceFeedback = false;
        }
    }
}
