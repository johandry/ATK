require_relative 'command'

require 'csv'

class Commands

	attr_accessor :commands

	def initialize(database, logger)
		@commands = Hash.new
		logger.info "Loading the commands from #{database}" if logger
		CSV.foreach(database) do |row|
			@commands[row[0]] = Command.new(row[0], row[1], row[2])
			logger.info "Command '#{row[0]}' loaded" if logger
		end
	end
end