#!/bin/bash

until rabbitmqctl status; do
  echo "Waiting for RabbitMQ..."
  sleep 5
done

rabbitmqctl set_policy DLX ".*" '{"dead-letter-exchange":"eshop_on_web-dlx"}' --apply-to queues

echo "RabbitMQ policy set."


