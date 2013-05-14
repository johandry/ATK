class Command
	attr_accessor :name, :command, :description

	def initialize (name, command, description)
		@name		= name
		@command 	= command
		@param_count= command.count "$"
		@description= description
	end

	def validate(*parameters)

	end

	def execute(*parameters)
		command = @command.gsub(/\$\d/) { |p| parameters[(p[1].to_i) - 1]}
		%x( #{command} )
	end

	def full_command(*parameters)
		@command.gsub(/\$\d/) { |p| parameters[(p[1].to_i) - 1]}
	end
end