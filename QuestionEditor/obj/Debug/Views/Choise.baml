   M S B A M L   `   `   `  ����I  EQuestionEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:QuestionEditorQuestionEditor  5   	       QuestionEditor.Choise   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:QuestionEditor     5   	   -   5          � Title$  Choise��5   	   $	��381��6   $	��669��6%    � 
ResizeMode$ NoResize=�61    � WindowStartupLocation$ CenterScreen=�6G    � WindowStyle$
 None=�6l   .��5	      � .+�5
   
   �� ��5      �� 5       �� ImageSource$J D/QuestionEditor;component/Images/white_and_grey_triangles_detail.jpg��6   .+�5      � $��40��6   ���	#FF3A3A3A$	��Top=�66   .+�5      �� $B��<Редактирование вариантов ответа��6   
�������$��Center=�6V   $��18(�6q   $��10,0,0,0q�6   5      �� 5       �� Orientation$ 
Horizontal=�6   $��10,10,10,10q�62   .+�5      �� .+�5      �� -   5         ��Name$ choices��5      $
��Left=�6/   $	��240��6J   $	��Top=�6W   $	��263��6o   �� 5      $ 
Horizontal=�6!   .+�5      �� -   5        $��IsRandom��5   #   $(��"Случайный порядок?��63    d� 	IsChecked$
 TrueY�6P   �� 5      -   5         ��Name$ button1��5   !   $��+��62   $��20(�6>   $��96,0,0,0q�6L   $��30��6^   $��30��6i   ���	#FF81C771
�������6�   5      5      �� 5      $��10,0,0,0q�6   .+�5      �� -   5        	 ��Name$	 questionText��5      $
��Left=�64   
 �� AcceptsReturn
 . $	��169��6d    �� TextWrapping$
 Wrap=�6q   $��TextBox��6�   $	��Top=�6�   $	��373��6�   �� 5      -   5        $ button��5      $��Сохранить��6-   $��Right=�6A   $	��Top=�6]   $��30��6u   $��75��6�   ���	#FF81C771
�������6�   5      5      �� 5      -   5        $ button2��5      $��Готово��6&   $��Right=�67   $��0,0,20,0q�6S   $	��Top=�6e   $��75��6}   $��30��6�   ���	#FF81C771
�������6�   5      5       5!      