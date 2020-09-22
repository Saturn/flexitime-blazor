using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Flexitime.Blazor
{
    public class FlexitimeValidator : ComponentBase
    {
        private ValidationMessageStore _messageStore;

        [CascadingParameter]
        private EditContext CurrentEditContext { get; set; }

        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(FlexitimeValidator)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. " +
                    $"For example, you can use {nameof(FlexitimeValidator)} " +
                    $"inside an {nameof(EditForm)}.");
            }

            _messageStore = new ValidationMessageStore(CurrentEditContext);

            CurrentEditContext.OnValidationRequested += (s, e) =>
                _messageStore.Clear();
            CurrentEditContext.OnFieldChanged += (s, e) =>
                _messageStore.Clear(e.FieldIdentifier);
        }

        public void DisplayErrors(Dictionary<Day, List<string>> errors)
        {
            Dictionary<string, List<string>> d = errors.ToDictionary(x => x.Key.ToString(), x => x.Value);
            DisplayErrors(d);
        }

        public void DisplayErrors(Dictionary<string, List<string>> errors)
        {
            foreach ((string key, List<string> value) in errors)
            {
                _messageStore.Add(CurrentEditContext.Field(key), value);
            }

            CurrentEditContext.NotifyValidationStateChanged();
        }

        public void ClearErrors()
        {
            _messageStore.Clear();
            CurrentEditContext.NotifyValidationStateChanged();
        }
    }
}
