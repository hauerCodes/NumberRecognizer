﻿//-----------------------------------------------------------------------
// <copyright file="GroupedImagesPageViewModel.cs" company="FH Wr.Neustadt">
//     Copyright Markus Zytek. All rights reserved.
// </copyright>
// <author>Markus Zytek</author>
// <summary>Grouped Images Page ViewModel.</summary>
//-----------------------------------------------------------------------
namespace NumberRecognizer.App.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight;
    using Mutzl.MvvmLight;
    using NumberRecognizer.App.DataModel;
    using NumberRecognizer.App.NumberRecognizerService;
    using NumberRecognizer.App.View;
    using NumberRecognizer.Cloud.Contract.Data;
    using PropertyChanged;

    /// <summary>
    /// Grouped Images Page ViewModel.
    /// </summary>
    [ImplementPropertyChanged]
    public class GroupedImagesPageViewModel : ViewModelBase
    {
        /// <summary>
        /// The images.
        /// </summary>
        private ObservableCollection<LocalTrainingImage> images;

        /// <summary>
        /// The selected image.
        /// </summary>
        private LocalTrainingImage selectedImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupedImagesPageViewModel"/> class.
        /// </summary>
        public GroupedImagesPageViewModel()
        {
            this.Images = new ObservableCollection<LocalTrainingImage>();
            this.ImageGroups = new ObservableCollection<TrainingImageGroup>();
            this.InitializeCommands();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupedImagesPageViewModel" /> class.
        /// </summary>
        /// <param name="images">The images.</param>
        public GroupedImagesPageViewModel(ObservableCollection<LocalTrainingImage> images)
            : this()
        {
            this.Images = images;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is image selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is image selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsImageSelected
        {
            get
            {
                return this.SelectedImage != null;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is loading.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is loading; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoading { get; set; }

        /// <summary>
        /// Gets or sets the name of the network.
        /// </summary>
        /// <value>
        /// The name of the network.
        /// </value>
        public string NetworkName { get; set; }

        /// <summary>
        /// Gets or sets the images.
        /// </summary>
        /// <value>
        /// The images.
        /// </value>
        public ObservableCollection<LocalTrainingImage> Images
        {
            get
            {
                return this.images;
            }

            set
            {
                this.images = value;
                this.GroupImages();
            }
        }

        /// <summary>
        /// Gets or sets the image groups.
        /// </summary>
        /// <value>
        /// The image groups.
        /// </value>
        public ObservableCollection<TrainingImageGroup> ImageGroups { get; set; }

        /// <summary>
        /// Gets or sets the delete image command.
        /// </summary>
        /// <value>
        /// The delete image command.
        /// </value>
        public ICommand DeleteImageCommand { get; set; }

        /// <summary>
        /// Gets or sets the next command.
        /// </summary>
        /// <value>
        /// The next command.
        /// </value>
        public ICommand UploadCommand { get; set; }

        /// <summary>
        /// Gets or sets the selected image.
        /// </summary>
        /// <value>
        /// The selected image.
        /// </value>
        public LocalTrainingImage SelectedImage
        {
            get
            {
                return this.selectedImage;
            }

            set
            {
                this.selectedImage = value;
            }
        }

        /// <summary>
        /// Initializes the commands.
        /// </summary>
        private void InitializeCommands()
        {
            this.UploadCommand = new DependentRelayCommand(this.CreateNetwork, () => this.ImageGroups.Count == 10 && this.IsLoading == false, this, () => this.ImageGroups, () => this.IsLoading);
            this.DeleteImageCommand = new DependentRelayCommand(this.DeleteImage, () => this.SelectedImage != null && this.IsLoading == false, this, () => this.SelectedImage, () => this.IsLoading);
        }

        /// <summary>
        /// Groups the images.
        /// </summary>
        private void GroupImages()
        {
            this.ImageGroups = new ObservableCollection<TrainingImageGroup>();

            foreach (string pattern in this.Images.OrderBy(p => p.Image.Pattern).Select(p => p.Image.Pattern).Distinct().ToList())
            {
                TrainingImageGroup trainingImageGroup = new TrainingImageGroup(pattern, pattern);
                foreach (LocalTrainingImage trainingImageRT in this.images.Where(p => p.Image.Pattern.Equals(pattern)).ToList())
                {
                    trainingImageGroup.Images.Add(trainingImageRT);
                }

                this.ImageGroups.Add(trainingImageGroup);
            }
        }

        /// <summary>
        /// Creates the network.
        /// </summary>
        private async void CreateNetwork()
        {
            this.IsLoading = true;
            ObservableCollection<TrainingImage> trainingImages = new ObservableCollection<TrainingImage>();

            foreach (LocalTrainingImage trainingImageRT in this.Images)
            {
                trainingImages.Add(trainingImageRT.Image);
            }

            try
            {
                NumberRecognizerServiceClient serviceClient = new NumberRecognizerServiceClient();

                await serviceClient.CreateNetworkAsync(this.NetworkName, trainingImages);
                await serviceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            this.IsLoading = false;
            App.RootFrame.Navigate(typeof(GroupedNetworksPage));
        }

        /// <summary>
        /// Deletes the image.
        /// </summary>
        private void DeleteImage()
        {
            this.IsLoading = true;
            this.Images.Remove(this.SelectedImage);
            this.selectedImage = null;
            this.GroupImages();
            this.IsLoading = false;
        }
    }
}
