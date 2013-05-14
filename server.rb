#!/usr/bin/env ruby

require_relative 'commands'

require 'socket'
require 'logger'
require 'date'


logger   = Logger.new('ATK.log', 'weekly')
randomer = Random.new
commands = Commands.new('ATK_Commands.csv', logger)

server = TCPServer.new(3001)
puts "ATK Server is listening on port 3001"

loop {
	Thread.start(server.accept) do |session|
		id = DateTime.now.strftime('%d%m%y%H%M%S') + randomer.rand(10..99).to_s
		logger.info "Session [#{id}]: Connection open from #{session.peeraddr[2]} (#{session.peeraddr[3]})"
		
		cmd = session.gets.chomp
		param = session.gets.chomp
		logger.debug "Session [#{id}]: Request to execute command #{cmd} with parameter #{param}"

		command = commands.commands[cmd].full_command(param)
		logger.debug "Session [#{id}]: Executing command #{command}"
		
		results = commands.commands[cmd].execute(param)
		session.puts "ATK: Command results: #{results}"
		session.puts "ATK: End"
		logger.debug "Session [#{id}]: Command results: #{results}"

		session.close		
		logger.info "Session [#{id}]: Connection closed from #{session.peeraddr[2]} (#{session.peeraddr[3]})"
	end
}

puts "ATK Server has been stopped"
logger.close