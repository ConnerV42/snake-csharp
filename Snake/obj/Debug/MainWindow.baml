   M S B A M L   `   `   `  ����@  <Snake, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:SnakeSnake  5   	       Snake.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:Snake     5   	   -   5          � Title$   Disintegrating Snake��5   	   $	��700��6'   $
��1000��64    � 
ResizeMode$ NoResize=�6A   .��5	      � 
����d�.+�5
   
   �� -   5         ��Name$ 	gameBoard��5
      
�������$
��Left=�67   $	��565��6R   $��50,50,50,50q�6_   $	��Top=�6t   $	��836��6�   �� 5   
   -   5         ��Name$ btnStart��5      $��
Start Game��6#   $
��Left=�68   $��
902,50,0,0q�6S   $	��Top=�6g   $��74��6    J� RenderTransformOrigin$ 0.473,-0.375&�6�   6�   �� 5   
   -   5        $ btnPause��5      $��
Pause Game��6#   $
��Left=�68   $��
902,75,0,0q�6S   $	��Top=�6g   $��76��6   $ 	0.73,-0.2&�6�   �� 5   
   -   5         ��Name$ currentScoreDisplay��5      $
��Left=�61   $��	75,14,0,0q�6L   $
��Wrap=�6_   $��Current Score: ��6s   $��20(�6�   $��Consolas)�6�   $��	ExtraBold"�6�   $	��Top=�6�   $��25��6�   $	��250��6�   �� 5   
   -   5        $ highScoreDisplay��5      $
��Left=�6.   $��
500,14,0,0q�6I   $
��Wrap=�6]   $��High Score: ��6q   $��20(�6�   $��Consolas)�6�   $��	ExtraBold"�6�   $	��Top=�6�   $��25��6�   $	��250��6�   �� 5   
   -   5        $ btnLoad��5      $��	Load Game��6"   $
��Left=�66   $��
50,627,0,0q�6Q   $	��Top=�6e   $��74��6}   6�   �� 5   
   -   5        $ btnSave��5      $��	Save Game��6"   $
��Left=�66   $��129,627,0,0q�6Q   $	��Top=�6f   $��75��6~   6�   5      5      