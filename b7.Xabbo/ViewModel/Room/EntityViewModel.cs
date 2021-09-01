﻿using System;
using System.Windows.Media;

using GalaSoft.MvvmLight;

using Xabbo.Core;

namespace b7.Xabbo.ViewModel
{
    public class EntityViewModel : ObservableObject
    {
        public IEntity Entity { get; }

        public int Index => Entity.Index;
        public long Id => Entity.Id;

        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string _figure;
        public string Figure
        {
            get => _figure;
            set
            {
                if (Set(ref _figure, value))
                    RaisePropertyChanged(nameof(AvatarImageUrl));
            }
        }

        private string _motto;
        public string Motto
        {
            get => _motto;
            set => Set(ref _motto, value);
        }

        private bool _isStaff;
        public bool IsStaff
        {
            get => _isStaff;
            set
            {
                if (Set(ref _isStaff, value))
                    UpdateVisualGroup();
            }
        }

        private bool _isAmbassador;
        public bool IsAmbassador
        {
            get => _isAmbassador;
            set
            {
                if (Set(ref _isAmbassador, value))
                    UpdateVisualGroup();
            }
        }

        private bool _isRoomOwner;
        public bool IsRoomOwner
        {
            get => _isRoomOwner;
            set
            {
                if (Set(ref _isRoomOwner, value))
                    UpdateVisualGroup();
            }
        }

        private int _controlLevel;
        public int ControlLevel
        {
            get => _controlLevel;
            set
            {
                if (Set(ref _controlLevel, value))
                {
                    RaisePropertyChanged(nameof(HasRights));
                    UpdateVisualGroup();
                }
            }
        }

        public bool HasRights => ControlLevel > 0;

        private string _imageSource = string.Empty;
        public string ImageSource
        {
            get => _imageSource;
            set => Set(ref _imageSource, value);
        }

        public string VisualGroupName
        {
            get
            {
                if (Entity.Type == EntityType.Pet)
                {
                    return "Pets";
                }
                else if (Entity.Type == EntityType.PrivateBot || Entity.Type == EntityType.PublicBot)
                {
                    return "Bots";
                }
                else
                {
                    if (IsRoomOwner)
                    {
                        return "Owner";
                    }
                    else if (IsStaff)
                    {
                        return "Staff";
                    }
                    else if (IsAmbassador)
                    {
                        return "Ambassadors";
                    }
                    else if (HasRights)
                    {
                        return ControlLevel switch
                        {
                            4 => "Owner",
                            3 => "Group Admin",
                            2 or 1 => "Room Rights",
                            _ => $"{ControlLevel}"
                        };
                    }
                    else
                        return "Users";
                }
            }
        }

        public int VisualGroupSort
        {
            get
            {
                if (IsStaff) return 0;
                else if (IsAmbassador) return 1;
                else if (IsRoomOwner || ControlLevel == 4) return 2;
                else if (HasRights) return 3;
                else
                {
                    switch (Entity.Type)
                    {
                        case EntityType.User: return 4;
                        case EntityType.Pet: return 5;
                        case EntityType.PublicBot: return 6;
                        case EntityType.PrivateBot: return 6;
                        default: break;
                    }
                }

                return 100;
            }
        }

        private Color _headerColor = Colors.Red;
        public Color HeaderColor
        {
            get => _headerColor;
            set => Set(ref _headerColor, value);
        }

        private bool _isIdle;
        public bool IsIdle
        {
            get => _isIdle;
            set => Set(ref _isIdle, value);
        }

        private bool _isTrading;
        public bool IsTrading
        {
            get => _isTrading;
            set => Set(ref _isTrading, value);
        }

        public string AvatarImageUrl
        {
            get
            {
                if (Entity.Type == EntityType.Pet)
                    return "";

                return $"https://www.habbo.com/habbo-imaging/avatarimage" +
                    $"?size=m" +
                    $"&figure={Figure}" +
                    $"&direction=4" +
                    $"&head_direction=4";
            }
        }

        public EntityViewModel(IEntity entity)
        {
            _name = entity.Name;
            Entity = entity;
            _motto = entity.Motto;
            _figure = entity.Figure;
        }

        private void UpdateVisualGroup()
        {
            RaisePropertyChanged(nameof(VisualGroupName));
            RaisePropertyChanged(nameof(VisualGroupSort));
        }
    }
}
