   M S B A M L   `   `   `  ���� I  EQuestionEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:QuestionEditorQuestionEditor      QuestionEditor.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:QuestionEditor     -     �Name$  window�� � Title$ 
MainWindow��$��738.89��$
��1093�� � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=� � 
ResizeMode$ NoResize=� � Icon$/ )/QuestionEditor;component/Images/logo.PNG�� � WindowStyle$
 None=�.��� -   $��MainGrid������  �� ImageSource$J D/QuestionEditor;component/Images/white_and_grey_triangles_detail.jpg��.+�� -   $��Hider��
���   � K� ZIndex$ 1��	 J� Opacity$		 0.7Q�$y�Hidden=��� .+�� -   $��UpLine��$��40�����	#FF3A3A3A.+��� $/��)/QuestionEditor;component/Images/logo.PNG��$
��Left=�$��5,0,0,0q��� -   $��	FileTitle��$��Center=�
�������$!��Новый документ��$��Center=�$��18(��� -   $��x��$��30��$��30��$��0,5,5,0q�$��Right=����	#FFE85151
�������$��16(�$	��Top=���Z� �� -   $	��◻��$��30��$��30��$��0,5,35,0q�$��Right=����	#FF5B5B5B
�������$��16(�$	��Top=���Z� �� -   $��_��$��30��$��30��$��0,0,65,0q�$��Right=����	#FFA8A8A8
�������$��16(���Z� o� -	   $��MenuLine��$��Stretch=�$��22��$	��Top=����	#FF3A3A3A���	#FFE2E2E2.@�m� $��Файл�����	#FFE2E2E2���	#FF3A3A3A.@�m� -
   $��Создать�����	#FF212121m� -   $��Открыть�����	#FF212121m� -   $��Сохранить�����	#FF212121m� -   $"��Сохранить как...�����	#FF212121m� -   $��
Выход�����	#FF212121m� $��Просмотр�����	#FFE2E2E2.@�m� -   $��Препросмотр�����	#FF212121m� -   $��Конвертация�����	#FFE2E2E2���	#FF3A3A3Am� $��Справка�����	#FFE2E2E2.@�m� -   $��Помощь�����	#FF212121m� -   $��Авторы�����	#FF212121�� 
 �� Orientation$
 
Horizontal=�$��Stretch=�$��0,10,0,0q�$��Stretch=�.+�� -   $��QuestionsGrid��$	��265��$��Stretch=�$��10,0,0,0q�.+��� $
 
Horizontal=�.+�� $	��250��$
��Left=�.+��� .+��� -   $��QuestionsStack��$	��600��.���� -    ��Name$ 	Questions��$��Stretch=�$��Stretch=��� -    ��Name$ button��$��+��$��0,-1,0,0q�$
��Left=�$��Bottom=����	#FF81C771
�������$��24(����	#FF9B9B9B$	��250���� -   $ button3��$��<��$��Center=�$
��Left=�$��15��$��75��
�������$��-1,0,0,0q����	#FFF1F1F1���	#FFACACAC� -   $��MiddleEditor��$��Stretch=�$��Stretch=�$��15,0,0,0q�{�.  $	��469��.+��� .+��� -    ��Name$ questionTitle��$��Stretch=�$	��Top=�$��21.333(��� $
 
Horizontal=�$��Stretch=�$��0,5,0,0q�.+��� -   $��30��$��30��
����������	#FFCFCFCF$
��Bold"�.���� $��15��$��15��$>��8/QuestionEditor;component/Images/glyphicons-103-bold.png���� -   $��30��$��30��
����������	#FFCFCFCF$
��Bold"�.���� $��15��$��15��$@��:/QuestionEditor;component/Images/glyphicons-102-italic.png���� -   $��30��$��30��
����������	#FFCFCFCF$
��Bold"�.���� $��15��$��15��$H��B/QuestionEditor;component/Images/glyphicons-104-text-underline.png��o� $��30,0,0,0q�
�������.@�m� $��30������ $>��8/QuestionEditor;component/Images/glyphicons-101-font.png��$��15��$��15��.@�m� -   $
��12px��m� -   $
��14px��m� -   $��16px (обычный)��m� -    $
��18px��m� -!   $��другой...��m� $��30������ $��15��$��15��$=��7/QuestionEditor;component/Images/glyphicons-93-tint.png��.@�m� -"   $��Черный�� m� Icon �� $��16��$��16��
���   �m� -#   $��Красный�� �� $��16��$��16�����	#FFFF3F3Fm� -$   $��
Синий�� �� $��16��$��16�����	#FF4467DEm� -%   $��Зеленый�� �� $��16��$��16�����	#FF47B23Dm� -&   $��Фиолетовый�� �� $��16��$��16�����	#FF6840F1m� -'   $��Оранжевый�� �� $��16��$��16�����	#FFF5780Fm� -(   $��другой...���� -)   $��30��$��30��$��30,0,0,0q�
����������	#FFCFCFCF$
��Bold"�.���� $��15��$��15��$D��>/QuestionEditor;component/Images/glyphicons-211-left-arrow.png���� -*   $��30��$��30��
����������	#FFCFCFCF$
��Bold"�.���� $E��?/QuestionEditor;component/Images/glyphicons-212-right-arrow.png��$��15��$��15���� -+   $ QuestionText��$��0,-1,0,0q� �� AcceptsReturn . $��Stretch=�$	��532�� �� TextWrapping$
 Wrap=�$	��Top=����	#FF9C9C9C� .+��� $
 
Horizontal=�$
��Left=�.+��� -,   $ input��$��
Текст��$	��Top=�$��83��$��40���� --   $ image��$��Картинка��$	��Top=�$��75��$��40���� -.   $ choise��$��
Выбор��$	��Top=�$��79��$��40���� -/   $��След. вопрос��$��85���� $
 
Horizontal=�$��Right=�.+��� -0   $ button2��$��Сохранить��$��94�����	#FF6BBF6B
�������$��40��$	��Top=��� -1   $ button1��$��x��$��43��$��24(�
����������	#FFF06D6D� -2   $��RemarksGrid��$	��210��$��10,0,0,0q�.+��� .+��� -3    ��Name$ QuestionNumber��$#��Текущий вопрос 1���� -4   $ QuestionCount��$#��Всего вопросов 0���� $��Заметки��$��0,16,0,0q�$��Right=��� -5   $��RemarksList��$	��534��.���� -6   $��Remarks���� -7   $��+��$��Bottom=�$��24(����	#FF81C771
��������� $��Bottom=�$��Right=����	#FF272727$��10(�$��0,0,5,5q�$A��;Powered by Uladzimir Paliukhovich and Uladzimir Papeka 2015��