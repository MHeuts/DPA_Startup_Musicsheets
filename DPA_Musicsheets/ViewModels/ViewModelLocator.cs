﻿using DPA_Musicsheets.Converters.Midi;
using DPA_Musicsheets.Converters.Midi.MidiEventHandlers;
using DPA_Musicsheets.IO;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace DPA_Musicsheets.ViewModels
{
    /// <summary>
    /// This is the MVVM-Light implementation of dependency injection.
    /// </summary>
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MusicLoader>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LilypondViewModel>();
            SimpleIoc.Default.Register<Staffs>();
            SimpleIoc.Default.Register<MidiPlayerViewModel>();

            SimpleIoc.Default.Register<MusicFileManager>();
            SimpleIoc.Default.Register<NoteFactory>();
            SimpleIoc.Default.Register<MidiMessageHandlerFactory>();
            SimpleIoc.Default.Register<MidiConverter>();
            SimpleIoc.Default.Register<MusicPlayer>();
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public LilypondViewModel LilypondViewModel => ServiceLocator.Current.GetInstance<LilypondViewModel>();
        public Staffs StaffsViewModel => ServiceLocator.Current.GetInstance<Staffs>();
        public MidiPlayerViewModel MidiPlayerViewModel => ServiceLocator.Current.GetInstance<MidiPlayerViewModel>();

        public static void Cleanup()
        {
            ServiceLocator.Current.GetInstance<MusicPlayer>().Cleanup();
        }
    }
}