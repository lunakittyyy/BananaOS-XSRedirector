﻿using System;
using System.Text.RegularExpressions;
using BananaOS;
using HarmonyLib;
using GorillaXS;

namespace BananaOS_XSRedirector.Patches
{
    [HarmonyPatch(typeof(BananaNotifications), "DisplayNotification", new Type[] { typeof (BananaNotifications.Notification) })]
    internal class BananaPatch
    {
        // technically removes anything wrapped in pointy brackets and not only richtext tags but i dont care 
        static Regex richTextTags = new Regex(@"<[^>]*>");
        private static bool Prefix(BananaNotifications __instance, ref BananaNotifications.Notification notification)
        {
            var modifiedText = notification.text;
            if (richTextTags.IsMatch(notification.text))
            {
                modifiedText = richTextTags.Replace(modifiedText, string.Empty);
            }
            Notifier.Notify("BananaOS", modifiedText, timeout: notification.duration, icon: "iVBORw0KGgoAAAANSUhEUgAAAQAAAAEACAYAAABccqhmAAAA0GVYSWZJSSoACAAAAAoAAAEEAAEAAAAAAQAAAQEEAAEAAAAAAQAAAgEDAAMAAACGAAAAEgEDAAEAAAABAAAAGgEFAAEAAACMAAAAGwEFAAEAAACUAAAAKAEDAAEAAAACAAAAMQECAA0AAACcAAAAMgECABQAAACqAAAAaYcEAAEAAAC+AAAAAAAAAAgACAAIAEgAAAABAAAASAAAAAEAAABHSU1QIDIuMTAuMzgAADIwMjU6MDM6MDYgMTA6MDQ6MzMAAQABoAMAAQAAAAEAAAAAAAAA/UWpLQAAAYRpQ0NQSUNDIHByb2ZpbGUAAHicfZE9SMNAGIbfpmpFKh3sICKYoTrZRUUctQpFqBBqhVYdTC79gyYNSYqLo+BacPBnserg4qyrg6sgCP6AuAtOii5S4ndJoUWMB3f38N73vtx9BwiNCtOsrllA020znUyI2dyqGHpFED2I0DoiM8uYk6QUfMfXPQJ8v4vzLP+6P0e/mrcYEBCJZ5lh2sQbxNObtsF5nzjKSrJKfE48btIFiR+5rnj8xrnossAzo2YmPU8cJRaLHax0MCuZGvEUcUzVdMoXsh6rnLc4a5Uaa92TvzCc11eWuU5zGEksYgkSRCiooYwKbMRp10mxkKbzhI9/yPVL5FLIVQYjxwKq0CC7fvA/+N1bqzA54SWFE0D3i+N8jAKhXaBZd5zvY8dpngDBZ+BKb/urDWDmk/R6W4sdAZFt4OK6rSl7wOUOMPhkyKbsSkGaQqEAvJ/RN+WAgVugb83rW+scpw9AhnqVugEODoGxImWv+7y7t7Nv/9a0+vcDMQJyjOZmN8gAAA14aVRYdFhNTDpjb20uYWRvYmUueG1wAAAAAAA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/Pgo8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJYTVAgQ29yZSA0LjQuMC1FeGl2MiI+CiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPgogIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiCiAgICB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIKICAgIHhtbG5zOnN0RXZ0PSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VFdmVudCMiCiAgICB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iCiAgICB4bWxuczpHSU1QPSJodHRwOi8vd3d3LmdpbXAub3JnL3htcC8iCiAgICB4bWxuczp0aWZmPSJodHRwOi8vbnMuYWRvYmUuY29tL3RpZmYvMS4wLyIKICAgIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIKICAgeG1wTU06RG9jdW1lbnRJRD0iZ2ltcDpkb2NpZDpnaW1wOjIwM2JiNWU0LWE5YTAtNGFlMC05MDFjLTJmMmFiNzA5MmQ4MSIKICAgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDpmNWFjMjFjYi1lZmFiLTQ5MWEtYWYxZS03Nzc4OTc2OGNmYTAiCiAgIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDpkMTk4ZTc2MS0zYTgzLTQxNTYtYjgwZS0yM2ZkZmFiMGU0ZjAiCiAgIGRjOkZvcm1hdD0iaW1hZ2UvcG5nIgogICBHSU1QOkFQST0iMi4wIgogICBHSU1QOlBsYXRmb3JtPSJMaW51eCIKICAgR0lNUDpUaW1lU3RhbXA9IjE3NDEyNzM0NzM5MTExNDYiCiAgIEdJTVA6VmVyc2lvbj0iMi4xMC4zOCIKICAgdGlmZjpPcmllbnRhdGlvbj0iMSIKICAgeG1wOkNyZWF0b3JUb29sPSJHSU1QIDIuMTAiCiAgIHhtcDpNZXRhZGF0YURhdGU9IjIwMjU6MDM6MDZUMTA6MDQ6MzMtMDU6MDAiCiAgIHhtcDpNb2RpZnlEYXRlPSIyMDI1OjAzOjA2VDEwOjA0OjMzLTA1OjAwIj4KICAgPHhtcE1NOkhpc3Rvcnk+CiAgICA8cmRmOlNlcT4KICAgICA8cmRmOmxpCiAgICAgIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiCiAgICAgIHN0RXZ0OmNoYW5nZWQ9Ii8iCiAgICAgIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6ZDI3MmEwZjEtOGZkMi00YzBjLTk2NzQtNGI0MWY0NjU2N2IwIgogICAgICBzdEV2dDpzb2Z0d2FyZUFnZW50PSJHaW1wIDIuMTAgKExpbnV4KSIKICAgICAgc3RFdnQ6d2hlbj0iMjAyNS0wMy0wNlQxMDowNDozMy0wNTowMCIvPgogICAgPC9yZGY6U2VxPgogICA8L3htcE1NOkhpc3Rvcnk+CiAgPC9yZGY6RGVzY3JpcHRpb24+CiA8L3JkZjpSREY+CjwveDp4bXBtZXRhPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgCjw/eHBhY2tldCBlbmQ9InciPz6Z1uqdAAAABmJLR0QAAAAAAAD5Q7t/AAAACXBIWXMAAAsTAAALEwEAmpwYAAAAB3RJTUUH6QMGDwQhvA1XgQAAAzNJREFUeNrt3cGqwjAQQNFW/ONS8RPEn65rt9Nm0knO2T/wRblMoUPWbV+OBZjSwxGAAAACAAgAIACAAAACAAgAIACAAAACAAgAIACAAAACAAgAIACAAAACAAgAIACAAAACALT2dAT5Pt/Y371fzg4TACAAgAAAAgAIACAAgAAAAgAIACAAgAAAAgACAEzJNuBJkc2+6FafLUJMAIAAAAIACAAgAIAAAAIACAAgAIAAAAIACAAIADCpdduXY9R/Lro9d1e2+jABAAIACAAgAIAAAAIACAAgAIAAAAIACAAgACAAwKTK3A2YeQdf688FJgBAAAABAAQAEABAAAABAAQAEADgMk9HUEf0rUP3CWACAAQAEABAAEAAAAEABAAQAEAAAAEABAAQAEAAgOLStwFttMVFz6D13QW+GxMAIACAAAACAAgAIACAAAACAAgAIACAAAACAAgA0Ji7ATvI3uprva3Xetuw97mbAAABAAQAEABAAAABAAQAEABAAAABAAQAEABAAIDubAMW4m7Ac6psLWaeuwkAPAIAAgAIACAAgAAAAgAIACAAgAAAAgAIAFCLZaCTIgsZ2Vd8uRos5xwqnp8JADwCAAIACAAgAIAAAAIACAAgAIAAAAIACAAgAEARtgE7uOsVX9HP6WqwPr8HEwAgAIAAAAIACAAgAIAAAAIACAAgADA7rwIXkv3K6GivzrogxQQACAAgACAAgAAAAgAIACAAgAAAAgAIACAAgAAAZa3bvhwVPmhk06rKhRW0/y1kqPh7MwGARwBAAAABAAQAEABAAAABAAQAEABAAAABAGopswwUEV0asUQ09+9hpu/fBAAeAQABAAQAEABAAAABAAQAEABAAAABAAQAEACgiKG3AaNaXz1l2zDnnJ27CQAQAEAAAAEABAAEwBGAAAACAAgAIACAAAACAAgAMAzbgB1kb8Hdle08EwAgAIAAAAIACAAgAIAAAAIACABwAW8CggkAEABAAAABAAQAEABAAAABAAQAEABAAAABAAQAEABAAAABAAQAEABAAAABAAQAEABAAAABAAQAEABAAAABAAQAEABAAAABAAQAEADgzw8/r0rzYKvTEgAAAABJRU5ErkJggg==");
            return false;
        }
    }
}