--просрочить старые брони (запускать раз в день)
SELECT shem.expire_old_reservations();

--завершить бронь при продаже
SELECT shem.complete_reservation(1);

--отменить бронь
SELECT shem.cancel_reservation(2);