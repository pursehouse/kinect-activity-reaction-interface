   M S B A M L   `   `   `  ����  BodyBasics-WPF    BodyBasicsWPF.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Body Basics��5   	    � Left$ 0��5   &   
 � Top$ 0��6/   $	��700��67   $	��700��6C    � 	Resources 5         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush��%�   &	        &	 #     &	 F     �� 5   
   $ 	#ff6e6e6e��62   �� 5	   
   $ 	#ff52318f��64   �� 5
   
   $ 	#ff00BCF2��62   5      .��5      � $
��10,0q�6    � ColumnDefinitions 5   
   �� 5      $
��113* �6   �� 5      $
��111* �6   .+�5   
   �� -   5         ��Name$ dogBar��5   2   $��Stretch=�6   $	��Top=�6B   
�������#��[  6m   $��10,0,-16,10q�6�   $��1��6�   $	��649��6�   .@�5      �� $��20(�68   ��6   �� *	DogName+68   5      � 5   
    � RowDefinitions 5      �� 5      $	��250 �6    �� 5      $	��250 �6    �� 5      $
��Auto �6    .+�5      (� $��0��6   $��Center=�6#   .��5      �� $��UniformToFill=�65   ��6   �� *	BodyImage+65   5      (� 5      $��1��6   $��Stretch=�6#   $��0,24,0,-150q�6A   $��2��6V   .��5      �� $
��None=�6?   $	��503��6N   $	��545��6Z   ��6   �� *ActionsHistoryImage+6Z   5      �� 5       -   5        $ 	statusBar��5    C   $��2��6   $��Stretch=�6%   $��Bottom=�6V   
�������#��[  6�   .@�5!      �� ��6    �� *
StatusText+5"      �� 5#      -   5        $��
Reload cfg��5#      $
��Left=�6*   $��88,2,0,0q�6E   $��1��6W   $	��Top=�6d   $��75��6|   6�   5$      5%      5&      