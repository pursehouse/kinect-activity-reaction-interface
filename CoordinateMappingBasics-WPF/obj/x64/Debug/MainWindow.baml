   M S B A M L   `   `   `  ����  CoordinateMappingBasics-WPF+    %CoordinateMappingBasicsWPF.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Coordinate Mapping Basics��5   	   $	��600��5   	   $	��700��6    � 	Resources 5         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  ScreenshotButton �� 
TargetType �� Orientation ��Name	 �� TextAlignment
 ��Name �� Triggers P� Property P� Value �� 
TargetName��%a  &	        &	 #     &	 F     '�� i     &	 �     7  7  �� 5	      $ 	#ff6e6e6e��6.   �� 5
      $ 	#ff52318f��60   �� 5      $ 	#ff00BCF2��6.   �� 5      " ��6   .�5      ��  �@J�SnapsToDevicePixels . 6.   5      �� 5      " ��6   .�5      ��  � �� 5   
   �� 5      " ��6   .R�5      � .+�5      �� $ 
Horizontal=�6   
������ .+�5      �� $ ScreenshotText��6   #��z��64   $
	 Left=�6U   $��Center=�6k   8��  $��15(�6�   � 5      $��9,0,0,0q�6   .+�5      �� $
 ScreenshotNormal��6   $!��Images\ScreenshotNormal.png��64   $
��None=�6Y   $��Center=�6h   �� 5      $
 ScreenshotHover��6   $ ��Images\ScreenshotHover.png��63   $
��None=�6W   $��Center=�6f   $y�	Collapsed=�6�   5      5      5       5      P� 5       �@J�IsMouseOver . . �5      �� $ ScreenshotNormal��6A    � y�$ 	Collapsed=�6/   6A   �� 5      $ ScreenshotHover��6?    � y�$ Visible=�6/   6?   �� 5       8  $ ScreenshotText��6X    � ��5!      5#      5%   	   5&      5'      .��5(      � $��	10 0 10 0q�6	    � RowDefinitions 5)      �� 5*      $
��Auto �6   �� 5+      $��* �6   �� 5,      $
��Auto �6   �� 5-      $
��Auto �6   .+�5/      ]� $��0��6   $��0 0 0 20q�6   .+�50      �� $
��Left=�6   $��Images\Logo.png��6$   $
��Fill=�6=   $��32��6L   $��81��6X   $��0 10 0 5q�6c   �� 51      $��Right=�6   $��0 0 -1 0q�6)   $��Bottom=�6;   #��[  6V   $��Segoe UI)�6�   $��18(�6�   Coordinate Mapping Basics6�   �� 52      $��Images\Status.png��6   $
��None=�6)   $��Center=�68   $��0 0 0 5q�6U   53      (� 54      $��1��6   $��Center=�6   .��55   
   � -   5        $��CompositeImage��55      .+�56      �� $��Images\Background.png��6   $��UniformToFill=�63   �� 57      $��UniformToFill=�63   ��6   �� *ImageSource+63   58      59      �� 5:      -   5        $��2��5:      #��[ 6   $��
Screenshot��6D   $
��Auto��6Y   $��Right=�6g   $��Center=�6�   $��
10 10 0 10q�6�   6�   �� 5;      -   5        $��	statusBar��5;   ;   $��3��6   $��Stretch=�6   $��Bottom=�6L   
�������#��[  6z   .@�5<      �� ��6   �� *
StatusText+5=      5>      5?      